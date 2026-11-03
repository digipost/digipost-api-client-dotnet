using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Digipost.Api.Client.Internal
{
    internal class TokenProvider : IDisposable
    {
        private static readonly TimeSpan RefreshMargin = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan MinimumCacheTime = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan FallbackTokenLifetime = TimeSpan.FromSeconds(60);

        private readonly ClientConfig _clientConfig;
        private readonly JwtAuthConfig _jwtAuthConfig;
        private readonly ILogger<DigipostClient> _logger;
        private readonly HttpClient _tokenClient;
        private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);

        private volatile CachedToken _cachedToken;

        public TokenProvider(ClientConfig clientConfig, JwtAuthConfig jwtAuthConfig, ILoggerFactory loggerFactory)
            : this(clientConfig, jwtAuthConfig, loggerFactory, CreateMtlsHttpMessageHandler(jwtAuthConfig.EnterpriseCertificate, clientConfig.WebProxy, clientConfig.Credential))
        {
        }

        internal TokenProvider(ClientConfig clientConfig, JwtAuthConfig jwtAuthConfig, ILoggerFactory loggerFactory, HttpMessageHandler tokenHttpMessageHandler)
        {
            _clientConfig = clientConfig;
            _jwtAuthConfig = jwtAuthConfig;
            _logger = loggerFactory.CreateLogger<DigipostClient>();
            _tokenClient = new HttpClient(tokenHttpMessageHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(clientConfig.TimeoutMilliseconds)
            };
        }

        private Uri TokenEndpoint => _jwtAuthConfig.TokenEndpoint ?? _clientConfig.Environment.TokenEndpoint;

        public void Dispose()
        {
            _tokenClient.Dispose();
            _refreshLock.Dispose();
        }

        public void InvalidateToken(string accessToken)
        {
            var current = _cachedToken;
            if (current != null && current.AccessToken == accessToken)
            {
                _cachedToken = null;
            }
        }

        public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            var cached = _cachedToken;
            if (cached != null && !cached.IsNearExpiry(DateTimeOffset.UtcNow))
            {
                return cached.AccessToken;
            }

            await _refreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                cached = _cachedToken;
                if (cached != null && !cached.IsNearExpiry(DateTimeOffset.UtcNow))
                {
                    return cached.AccessToken;
                }

                var refreshed = await FetchTokenAsync(cancellationToken).ConfigureAwait(false);
                _cachedToken = refreshed;

                return refreshed.AccessToken;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private async Task<CachedToken> FetchTokenAsync(CancellationToken cancellationToken)
        {
            var tokenEndpoint = TokenEndpoint;

            var requestContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _jwtAuthConfig.ClientId),
                new KeyValuePair<string, string>("scope", "dpost-api:" + _clientConfig.Broker.Id),
                // mIDP matches "resource" as an exact string without a trailing slash, unlike Environment.Url.
                new KeyValuePair<string, string>("resource", _clientConfig.Environment.Url.AbsoluteUri.TrimEnd('/'))
            });

            using (var response = await _tokenClient.PostAsync(tokenEndpoint, requestContent, cancellationToken).ConfigureAwait(false))
            {
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Token endpoint {TokenEndpoint} returned HTTP {StatusCode}: {Body}", tokenEndpoint, (int) response.StatusCode, responseBody);
                    throw new ApiException($"Token endpoint returned HTTP {(int) response.StatusCode} for {tokenEndpoint}: {responseBody}");
                }

                return ParseTokenResponse(responseBody, DateTimeOffset.UtcNow);
            }
        }

        internal static CachedToken ParseTokenResponse(string json, DateTimeOffset now)
        {
            JObject tokenResponse;
            try
            {
                tokenResponse = JObject.Parse(json);
            }
            catch (Exception e)
            {
                throw new ApiException("Could not parse token endpoint response as JSON", e);
            }

            var accessToken = tokenResponse["access_token"]?.ToString();
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ApiException("Token endpoint response did not contain an 'access_token' field");
            }

            var expiresAt = ResolveExpiry(accessToken, tokenResponse, now);
            var cacheValidUntil = ResolveCacheValidUntil(now, expiresAt);

            return new CachedToken(accessToken, expiresAt, cacheValidUntil);
        }

        /// <summary>
        ///     Caches a token until shortly before it expires, but never for less than <see cref="MinimumCacheTime" /> -
        ///     otherwise a short-lived token (at or below <see cref="RefreshMargin" />) would be refetched on every single
        ///     call to <see cref="GetTokenAsync" />, hammering the token endpoint.
        /// </summary>
        internal static DateTimeOffset ResolveCacheValidUntil(DateTimeOffset now, DateTimeOffset expiresAtUtc)
        {
            var refreshAt = expiresAtUtc - RefreshMargin;
            var minimum = now + MinimumCacheTime;

            if (refreshAt > minimum)
            {
                return refreshAt;
            }

            return minimum < expiresAtUtc ? minimum : expiresAtUtc;
        }

        private static DateTimeOffset ResolveExpiry(string accessToken, JObject tokenResponse, DateTimeOffset now)
        {
            var expiresIn = tokenResponse["expires_in"];
            if (expiresIn != null && expiresIn.Type == JTokenType.Integer)
            {
                return now.AddSeconds(expiresIn.Value<long>());
            }

            var exp = TryReadJwtExpClaim(accessToken);
            if (exp.HasValue)
            {
                return DateTimeOffset.FromUnixTimeSeconds(exp.Value);
            }

            return now.Add(FallbackTokenLifetime);
        }

        internal static long? TryReadJwtExpClaim(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length < 2)
                {
                    return null;
                }

                var payloadJson = Base64UrlDecode(parts[1]);
                var payload = JObject.Parse(payloadJson);
                var exp = payload["exp"];

                return exp != null && exp.Type == JTokenType.Integer ? exp.Value<long>() : (long?) null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string Base64UrlDecode(string input)
        {
            var base64 = input.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            var bytes = Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        private static HttpClientHandler CreateMtlsHttpMessageHandler(X509Certificate2 enterpriseCertificate, WebProxy proxy, NetworkCredential credential)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(enterpriseCertificate);

            if (proxy != null)
            {
                proxy.Credentials = credential;
                handler.Proxy = proxy;
                handler.UseProxy = true;
                handler.UseDefaultCredentials = false;
            }

            return handler;
        }

        internal sealed class CachedToken
        {
            public CachedToken(string accessToken, DateTimeOffset expiresAtUtc, DateTimeOffset cacheValidUntilUtc)
            {
                AccessToken = accessToken;
                ExpiresAtUtc = expiresAtUtc;
                CacheValidUntilUtc = cacheValidUntilUtc;
            }

            public string AccessToken { get; }

            public DateTimeOffset ExpiresAtUtc { get; }

            public DateTimeOffset CacheValidUntilUtc { get; }

            public bool IsNearExpiry(DateTimeOffset now)
            {
                return now >= CacheValidUntilUtc;
            }
        }
    }
}
