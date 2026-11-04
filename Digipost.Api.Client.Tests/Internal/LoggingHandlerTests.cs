using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Tests.TestSupport;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Internal
{
    public class LoggingHandlerTests
    {
        public class SendAsyncMethod
        {
            [Fact]
            public async Task RedactsTheAuthorizationHeader_InsteadOfLoggingTheLiveBearerToken()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test) {LogRequestAndResponse = true};
                var loggerFactory = new RecordingLoggerFactory();

                var handler = new LoggingHandler(clientConfig, loggerFactory)
                {
                    InnerHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent("ok")})
                };
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Get, "http://fakeuri.no/someendpoint");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "a-live-access-token");

                await invoker.SendAsync(request, CancellationToken.None);

                Assert.Contains(loggerFactory.Messages, message => message.Contains("Authorization: [REDACTED]"));
                Assert.DoesNotContain(loggerFactory.Messages, message => message.Contains("a-live-access-token"));
            }

            [Fact]
            public async Task LogsEachAttempt_WhenPositionedInsideBearerTokenAuthenticationHandler_SoA401RetryIsAlsoLogged()
            {
                // LoggingHandler must sit inside BearerTokenAuthenticationHandler (not outside it) for this to hold -
                // see the handler ordering comment in DigipostClient's JWT/mTLS constructor.
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test) {LogRequestAndResponse = true};
                var jwtAuthConfig = new JwtAuthConfig("client-id", CertificateResource.Certificate());
                var loggerFactory = new RecordingLoggerFactory();

                StubHttpMessageHandler tokenHandler = null;
                tokenHandler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\":\"tok{tokenHandler.InvocationCount}\",\"expires_in\":3600}}", Encoding.UTF8, "application/json")
                });
                var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, loggerFactory, tokenHandler);

                var attempt = 0;
                var resourceHandler = new StubHttpMessageHandler(_ =>
                {
                    attempt++;
                    return attempt == 1
                        ? new HttpResponseMessage(HttpStatusCode.Unauthorized) {Content = new StringContent("unauthorized")}
                        : new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent("ok")};
                });

                var bearerHandler = new BearerTokenAuthenticationHandler(clientConfig, tokenProvider)
                {
                    InnerHandler = new LoggingHandler(clientConfig, loggerFactory) {InnerHandler = resourceHandler}
                };
                var invoker = new HttpMessageInvoker(bearerHandler);

                var request = new HttpRequestMessage(HttpMethod.Get, "http://fakeuri.no/someendpoint");

                var response = await invoker.SendAsync(request, CancellationToken.None);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(2, loggerFactory.Messages.Count(message => message.StartsWith("Outgoing:")));
                Assert.Equal(2, loggerFactory.Messages.Count(message => message.StartsWith("Incoming:")));
            }
        }
    }
}
