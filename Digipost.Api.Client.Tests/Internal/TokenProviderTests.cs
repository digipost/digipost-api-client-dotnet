using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Exceptions;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Tests.TestSupport;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Internal
{
    public class TokenProviderTests
    {
        public class ParseTokenResponseMethod
        {
            [Fact]
            public void UsesExpiresIn_WhenPresent()
            {
                var now = DateTimeOffset.UtcNow;

                var token = TokenProvider.ParseTokenResponse("{\"access_token\":\"abc\",\"expires_in\":3600}", now);

                Assert.Equal("abc", token.AccessToken);
                Assert.Equal(now.AddSeconds(3600), token.ExpiresAtUtc);
            }

            [Fact]
            public void FallsBackToJwtExpClaim_WhenExpiresInMissing()
            {
                var payload = Base64UrlEncode("{\"exp\":1700000000}");
                var jwt = $"header.{payload}.signature";

                var token = TokenProvider.ParseTokenResponse($"{{\"access_token\":\"{jwt}\"}}", DateTimeOffset.UtcNow);

                Assert.Equal(jwt, token.AccessToken);
                Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1700000000), token.ExpiresAtUtc);
            }

            [Fact]
            public void FallsBackToFixed60Seconds_WhenNeitherAvailable()
            {
                var now = DateTimeOffset.UtcNow;

                var token = TokenProvider.ParseTokenResponse("{\"access_token\":\"opaque-token\"}", now);

                Assert.Equal("opaque-token", token.AccessToken);
                Assert.Equal(now.AddSeconds(60), token.ExpiresAtUtc);
            }

            private static string Base64UrlEncode(string value)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(value))
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .TrimEnd('=');
            }
        }

        public class ResolveCacheValidUntilMethod
        {
            private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-08-14T12:00:00Z");

            [Fact]
            public void RefreshesShortlyBeforeExpiry()
            {
                var cacheValidUntil = TokenProvider.ResolveCacheValidUntil(Now, Now.AddSeconds(300));

                Assert.Equal(Now.AddSeconds(270), cacheValidUntil);
            }

            [Fact]
            public void CachesShortLivedTokens_InsteadOfRefetchingPerRequest()
            {
                var cacheValidUntil = TokenProvider.ResolveCacheValidUntil(Now, Now.AddSeconds(10));

                Assert.Equal(Now.AddSeconds(5), cacheValidUntil);
            }

            [Fact]
            public void NeverCachesLongerThanTheTokenIsValid()
            {
                var cacheValidUntil = TokenProvider.ResolveCacheValidUntil(Now, Now.AddSeconds(3));

                Assert.Equal(Now.AddSeconds(3), cacheValidUntil);
            }

            [Fact]
            public void AlwaysCachesForAPositiveTimespan()
            {
                Assert.True(TokenProvider.ResolveCacheValidUntil(Now, Now.AddSeconds(31)) > Now);
                Assert.True(TokenProvider.ResolveCacheValidUntil(Now, Now.AddSeconds(30)) > Now);
                Assert.True(TokenProvider.ResolveCacheValidUntil(Now, Now.AddSeconds(1)) > Now);
            }
        }

        public class GetTokenAsyncMethod
        {
            private static readonly ClientConfig ClientConfig = new ClientConfig(new Broker(1337), Environment.Test);
            private static readonly JwtAuthConfig JwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

            [Fact]
            public async Task CachesToken_DoesNotRefetch_BeforeExpiry()
            {
                var handler = new StubHttpMessageHandler(_ => JsonResponse("{\"access_token\":\"tok1\",\"expires_in\":3600}"));
                var provider = new TokenProvider(ClientConfig, JwtAuthConfig, new NullLoggerFactory(), handler);

                var token1 = await provider.GetTokenAsync();
                var token2 = await provider.GetTokenAsync();

                Assert.Equal("tok1", token1);
                Assert.Equal("tok1", token2);
                Assert.Equal(1, handler.InvocationCount);
            }

            [Fact]
            public async Task CachesShortLivedToken_ForAtLeastTheMinimumCacheTime()
            {
                // expires_in of 10 seconds is within the 30-second refresh margin, but the minimum cache
                // time keeps back-to-back calls from refetching on every single request.
                var handler = new StubHttpMessageHandler(_ => JsonResponse("{\"access_token\":\"tok1\",\"expires_in\":10}"));
                var provider = new TokenProvider(ClientConfig, JwtAuthConfig, new NullLoggerFactory(), handler);

                await provider.GetTokenAsync();
                await provider.GetTokenAsync();

                Assert.Equal(1, handler.InvocationCount);
            }

            [Fact]
            public async Task ConcurrentCallers_OnlyTriggerOneFetch()
            {
                var handler = new StubHttpMessageHandler(_ => JsonResponse("{\"access_token\":\"tok1\",\"expires_in\":3600}"));
                var provider = new TokenProvider(ClientConfig, JwtAuthConfig, new NullLoggerFactory(), handler);

                var tasks = Enumerable.Range(0, 20).Select(_ => provider.GetTokenAsync());
                var tokens = await Task.WhenAll(tasks);

                Assert.All(tokens, token => Assert.Equal("tok1", token));
                Assert.Equal(1, handler.InvocationCount);
            }

            [Fact]
            public async Task ThrowsApiException_OnNonSuccessResponse()
            {
                var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("invalid_client")
                });
                var provider = new TokenProvider(ClientConfig, JwtAuthConfig, new NullLoggerFactory(), handler);

                await Assert.ThrowsAsync<ApiException>(() => provider.GetTokenAsync());
            }

            private static HttpResponseMessage JsonResponse(string json)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            }
        }

        public class FetchTokenLoggingMethod
        {
            private static readonly JwtAuthConfig JwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

            [Fact]
            public async Task LogsOutgoingRequestAndIncomingResponse_WhenLogRequestAndResponseIsEnabled()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test) {LogRequestAndResponse = true};
                var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });
                var loggerFactory = new RecordingLoggerFactory();
                var provider = new TokenProvider(clientConfig, JwtAuthConfig, loggerFactory, handler);

                await provider.GetTokenAsync();

                Assert.Contains(loggerFactory.Messages, message => message.Contains("Outgoing token request") && message.Contains("client_id=client-id"));
                Assert.Contains(loggerFactory.Messages, message => message.Contains("Incoming token response") && message.Contains("200"));
            }

            [Fact]
            public async Task NeverLogsTheAccessToken()
            {
                // The response body - which for a successful request is itself a live, usable access token -
                // is deliberately never logged, not even redacted; only the status code is.
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test) {LogRequestAndResponse = true};
                var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });
                var loggerFactory = new RecordingLoggerFactory();
                var provider = new TokenProvider(clientConfig, JwtAuthConfig, loggerFactory, handler);

                await provider.GetTokenAsync();

                Assert.DoesNotContain(loggerFactory.Messages, message => message.Contains("tok1"));
            }

            [Fact]
            public async Task DoesNotLog_WhenLogRequestAndResponseIsDisabled()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test) {LogRequestAndResponse = false};
                var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });
                var loggerFactory = new RecordingLoggerFactory();
                var provider = new TokenProvider(clientConfig, JwtAuthConfig, loggerFactory, handler);

                await provider.GetTokenAsync();

                Assert.DoesNotContain(loggerFactory.Messages, message => message.Contains("token request") || message.Contains("token response"));
            }
        }

        public class DisposeMethod
        {
            [Fact]
            public void DisposesTheMtlsHttpMessageHandler()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());
                var trackingHandler = new DisposeTrackingHandler();

                var provider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), trackingHandler);

                provider.Dispose();

                Assert.True(trackingHandler.Disposed);
            }

            [Fact]
            public async Task DoesNotMaskTheRealOutcome_WithObjectDisposedException_WhenDisposedWhileGetTokenAsyncIsInFlight()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                // Gates the token endpoint response so the test controls exactly when GetTokenAsync's in-flight
                // call is genuinely suspended - letting it dispose the provider (and so _refreshLock) mid-flight
                // deterministically, rather than relying on real timing.
                var responseGate = new TaskCompletionSource<HttpResponseMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
                var gatedHandler = new GatedHttpMessageHandler(responseGate.Task);
                var provider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), gatedHandler);

                var getTokenTask = provider.GetTokenAsync();
                await gatedHandler.RequestReceived.Task;

                // Disposing TokenProvider disposes its HttpClient, which aborts this in-flight call with a
                // TaskCanceledException - that's real, expected HttpClient behavior, not the bug under test.
                // What's under test is that this real exception reaches the caller as-is, rather than being
                // replaced by an ObjectDisposedException from releasing the now-disposed _refreshLock.
                provider.Dispose();
                responseGate.TrySetResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });

                var exception = await Record.ExceptionAsync(() => getTokenTask);

                Assert.NotNull(exception);
                Assert.IsNotType<ObjectDisposedException>(exception);
            }
        }

        public class InvalidateTokenAsyncMethod
        {
            [Fact]
            public async Task WaitsForAnInFlightRefresh_SoItCannotClearAFreshlyInstalledToken()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                // Gates the token endpoint response so the test controls exactly when the in-flight refresh
                // (started below) is genuinely suspended while holding _refreshLock.
                var responseGate = new TaskCompletionSource<HttpResponseMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
                var gatedHandler = new GatedHttpMessageHandler(responseGate.Task);
                var provider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), gatedHandler);

                // Cache starts empty, so this refresh acquires _refreshLock and blocks on the token endpoint.
                var getTokenTask = provider.GetTokenAsync();
                await gatedHandler.RequestReceived.Task;

                // Invalidating some unrelated, already-stale token concurrently must wait for the same lock -
                // proving it can't race ahead of the in-flight refresh and clear whatever token that refresh
                // is about to install.
                var invalidateTask = provider.InvalidateTokenAsync("some-other-stale-token");
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.False(invalidateTask.IsCompleted);

                responseGate.SetResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });

                var freshToken = await getTokenTask;
                await invalidateTask;

                Assert.Equal("tok1", freshToken);

                // If InvalidateTokenAsync had wiped out the freshly-installed token, this would trigger a
                // second, unnecessary fetch from the token endpoint.
                var tokenAfterInvalidate = await provider.GetTokenAsync();
                Assert.Equal("tok1", tokenAfterInvalidate);
                Assert.Equal(1, gatedHandler.InvocationCount);
            }

            [Fact]
            public async Task ClearsTheCachedToken_WhenItStillMatches()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());
                StubHttpMessageHandler handler = null;
                handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\":\"tok{handler.InvocationCount}\",\"expires_in\":3600}}", Encoding.UTF8, "application/json")
                });
                var provider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), handler);

                var firstToken = await provider.GetTokenAsync();
                await provider.InvalidateTokenAsync(firstToken);
                var secondToken = await provider.GetTokenAsync();

                Assert.Equal("tok1", firstToken);
                Assert.Equal("tok2", secondToken);
                Assert.Equal(2, handler.InvocationCount);
            }

            [Fact]
            public async Task DoesNothing_WhenTheGivenTokenIsNotTheCurrentlyCachedOne()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());
                var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });
                var provider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), handler);

                await provider.GetTokenAsync();
                await provider.InvalidateTokenAsync("a-token-that-was-never-cached");
                var tokenAfterInvalidate = await provider.GetTokenAsync();

                Assert.Equal("tok1", tokenAfterInvalidate);
                Assert.Equal(1, handler.InvocationCount);
            }
        }

        private sealed class DisposeTrackingHandler : HttpMessageHandler
        {
            public bool Disposed { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }

            protected override void Dispose(bool disposing)
            {
                Disposed = true;
                base.Dispose(disposing);
            }
        }

        private sealed class GatedHttpMessageHandler : HttpMessageHandler
        {
            private readonly Task<HttpResponseMessage> _response;
            private int _invocationCount;

            public GatedHttpMessageHandler(Task<HttpResponseMessage> response)
            {
                _response = response;
            }

            public TaskCompletionSource<bool> RequestReceived { get; } = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            public int InvocationCount => _invocationCount;

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Interlocked.Increment(ref _invocationCount);
                RequestReceived.TrySetResult(true);
                return await _response.ConfigureAwait(false);
            }
        }
    }
}
