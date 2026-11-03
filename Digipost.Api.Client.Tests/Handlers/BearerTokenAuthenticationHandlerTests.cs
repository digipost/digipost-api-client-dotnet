using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Exceptions;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Tests.Fakes;
using Digipost.Api.Client.Tests.TestSupport;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Internal
{
    public class BearerTokenAuthenticationHandlerTests
    {
        public class SendAsyncMethod
        {
            [Fact]
            public async Task SetsAuthorizationHeaderAndCommonHeaders_ButNeverASignatureHeader()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                var tokenHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });
                var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), tokenHandler);

                var handler = new BearerTokenAuthenticationHandler(clientConfig, tokenProvider)
                {
                    InnerHandler = new FakeResponseHandler {ResultCode = HttpStatusCode.OK, HttpContent = new StringContent(string.Empty)}
                };
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Get, "http://fakeuri.no/someendpoint");

                await invoker.SendAsync(request, CancellationToken.None);

                Assert.Equal("Bearer", request.Headers.Authorization.Scheme);
                Assert.Equal("tok1", request.Headers.Authorization.Parameter);
                Assert.True(request.Headers.Contains("X-Digipost-UserId"));
                Assert.True(request.Headers.Contains("Date"));
                Assert.True(request.Headers.Contains("Accept"));
                Assert.True(request.Headers.Contains("User-Agent"));
                Assert.False(request.Headers.Contains("X-Digipost-Signature"));
            }

            [Fact]
            public async Task SetsContentHashHeader_WhenRequestHasContent()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                var tokenHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });
                var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), tokenHandler);

                var handler = new BearerTokenAuthenticationHandler(clientConfig, tokenProvider)
                {
                    InnerHandler = new FakeResponseHandler {ResultCode = HttpStatusCode.OK, HttpContent = new StringContent(string.Empty)}
                };
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Post, "http://fakeuri.no/someendpoint")
                {
                    Content = new StringContent("body")
                };

                await invoker.SendAsync(request, CancellationToken.None);

                Assert.True(request.Headers.Contains("X-Content-SHA256"));
            }

            [Fact]
            public async Task DisposesTheOriginalUnauthorizedResponse_EvenWhenRefreshingTheTokenForRetryThrows()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                // First call (the initial token fetch) succeeds; every call after that (the refresh
                // triggered by the 401 below) fails, simulating a token endpoint that's down on retry.
                StubHttpMessageHandler tokenHandler = null;
                tokenHandler = new StubHttpMessageHandler(_ =>
                    tokenHandler.InvocationCount == 1
                        ? new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")}
                        : new HttpResponseMessage(HttpStatusCode.InternalServerError) {Content = new StringContent("token endpoint down")});
                var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), tokenHandler);

                var trackedContent = new DisposeTrackingContent(string.Empty);
                var handler = new BearerTokenAuthenticationHandler(clientConfig, tokenProvider)
                {
                    InnerHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized) {Content = trackedContent})
                };
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Get, "http://fakeuri.no/someendpoint");

                await Assert.ThrowsAsync<ApiException>(() => invoker.SendAsync(request, CancellationToken.None));

                Assert.True(trackedContent.Disposed);
            }

            [Fact]
            public async Task RetriesOnceWithARefreshedToken_WhenInitialRequestIsUnauthorized()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                // Each call to the token endpoint returns a distinct token, so the test can prove the
                // retry actually carries a *refreshed* token rather than resending the stale one.
                StubHttpMessageHandler tokenHandler = null;
                tokenHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\":\"tok{tokenHandler.InvocationCount}\",\"expires_in\":3600}}", Encoding.UTF8, "application/json")
                });
                var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), tokenHandler);

                var capturedAuthorizationHeaders = new List<string>();
                var resourceHandler = new StubHttpMessageHandler(request =>
                {
                    capturedAuthorizationHeaders.Add(request.Headers.Authorization?.ToString());
                    return capturedAuthorizationHeaders.Count == 1
                        ? new HttpResponseMessage(HttpStatusCode.Unauthorized) {Content = new StringContent("unauthorized")}
                        : new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent("ok")};
                });

                var handler = new BearerTokenAuthenticationHandler(clientConfig, tokenProvider) {InnerHandler = resourceHandler};
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Get, "http://fakeuri.no/someendpoint");

                var response = await invoker.SendAsync(request, CancellationToken.None);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(2, resourceHandler.InvocationCount);
                Assert.Equal(2, tokenHandler.InvocationCount);
                Assert.Equal(new[] {"Bearer tok1", "Bearer tok2"}, capturedAuthorizationHeaders);
            }

            [Fact]
            public async Task DoesNotRetry_WhenResponseIsNotUnauthorized()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                var tokenHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"access_token\":\"tok1\",\"expires_in\":3600}", Encoding.UTF8, "application/json")
                });
                var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), tokenHandler);

                var resourceHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError) {Content = new StringContent("boom")});

                var handler = new BearerTokenAuthenticationHandler(clientConfig, tokenProvider) {InnerHandler = resourceHandler};
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Get, "http://fakeuri.no/someendpoint");

                var response = await invoker.SendAsync(request, CancellationToken.None);

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                Assert.Equal(1, resourceHandler.InvocationCount);
                Assert.Equal(1, tokenHandler.InvocationCount);
            }

            [Fact]
            public async Task RetriedRequest_PreservesTheOriginalMethodAndBody()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());

                StubHttpMessageHandler tokenHandler = null;
                tokenHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\":\"tok{tokenHandler.InvocationCount}\",\"expires_in\":3600}}", Encoding.UTF8, "application/json")
                });
                var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), tokenHandler);

                var capturedMethods = new List<string>();
                var capturedBodies = new List<string>();
                var resourceHandler = new StubHttpMessageHandler(request =>
                {
                    capturedMethods.Add(request.Method.Method);
                    capturedBodies.Add(request.Content?.ReadAsStringAsync().GetAwaiter().GetResult());
                    return capturedBodies.Count == 1
                        ? new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        : new HttpResponseMessage(HttpStatusCode.OK);
                });

                var handler = new BearerTokenAuthenticationHandler(clientConfig, tokenProvider) {InnerHandler = resourceHandler};
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Post, "http://fakeuri.no/someendpoint")
                {
                    Content = new StringContent("payload")
                };

                var response = await invoker.SendAsync(request, CancellationToken.None);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(new[] {"POST", "POST"}, capturedMethods);
                Assert.Equal(new[] {"payload", "payload"}, capturedBodies);
            }
        }

        private sealed class DisposeTrackingContent : StringContent
        {
            public DisposeTrackingContent(string content) : base(content)
            {
            }

            public bool Disposed { get; private set; }

            protected override void Dispose(bool disposing)
            {
                Disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}
