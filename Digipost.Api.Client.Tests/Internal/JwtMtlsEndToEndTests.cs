using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Tests.TestSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Handlers
{
    // Uses real local Kestrel HTTPS servers rather than handler-level mocks, since only a real TLS handshake
    // can prove which certificate (if any) actually got presented.
    public class JwtMtlsEndToEndTests
    {
        [Fact]
        public async Task TokenProvider_PresentsClientCertificate_ToTokenEndpoint()
        {
            var clientCertificate = SelfSignedCertificateFactory.CreateSelfSigned("CN=digipost-mtls-client-test");
            var midpCertificate = SelfSignedCertificateFactory.CreateSelfSigned("CN=digipost-mtls-midp-test");

            await using var tokenServer = await MtlsTestServer.StartAsync(midpCertificate, async context =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"access_token\":\"end-to-end-token\",\"expires_in\":3600}");
            });

            var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
            var jwtAuthConfig = new JwtAuthConfig("client-id", clientCertificate)
            {
                TokenEndpoint = tokenServer.BaseAddress
            };

            var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), CreateTestTokenHandler(clientCertificate, midpCertificate));

            var token = await tokenProvider.GetTokenAsync();

            Assert.Equal("end-to-end-token", token);
            Assert.Single(tokenServer.RecordedRequests);

            var recordedRequest = tokenServer.RecordedRequests[0];
            Assert.NotNull(recordedRequest.ClientCertificate);
            Assert.Equal(clientCertificate.Thumbprint, recordedRequest.ClientCertificate.Thumbprint);
        }

        [Fact]
        public async Task JwtMtlsHttpPipeline_PresentsBearerToken_ButNoClientCertificate_OnApiCalls()
        {
            var clientCertificate = SelfSignedCertificateFactory.CreateSelfSigned("CN=digipost-mtls-client-test");
            var midpCertificate = SelfSignedCertificateFactory.CreateSelfSigned("CN=digipost-mtls-midp-test");
            var apiCertificate = SelfSignedCertificateFactory.CreateSelfSigned("CN=digipost-mtls-api-test");

            await using var tokenServer = await MtlsTestServer.StartAsync(midpCertificate, async context =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"access_token\":\"end-to-end-token\",\"expires_in\":3600}");
            });

            // AllowCertificate (not Require) so the assertion below can prove none is actually sent.
            await using var resourceServer = await MtlsTestServer.StartAsync(apiCertificate, context =>
            {
                context.Response.StatusCode = 200;
                return Task.CompletedTask;
            }, ClientCertificateMode.AllowCertificate);

            var environment = Environment.Test;
            environment.Url = resourceServer.BaseAddress;

            var clientConfig = new ClientConfig(new Broker(1337), environment);
            var jwtAuthConfig = new JwtAuthConfig("client-id", clientCertificate)
            {
                TokenEndpoint = tokenServer.BaseAddress
            };

            var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, new NullLoggerFactory(), CreateTestTokenHandler(clientCertificate, midpCertificate));

            var httpClient = HttpClientFactory.Create(CreateResourceServerHandler(apiCertificate), new BearerTokenAuthenticationHandler(clientConfig, tokenProvider));
            httpClient.BaseAddress = environment.Url;

            var response = await httpClient.GetAsync("some/path");

            Assert.True(response.IsSuccessStatusCode);
            Assert.Single(resourceServer.RecordedRequests);

            var recordedRequest = resourceServer.RecordedRequests[0];
            Assert.Null(recordedRequest.ClientCertificate);
            Assert.Equal("Bearer end-to-end-token", recordedRequest.Headers["Authorization"]);
            Assert.False(recordedRequest.Headers.ContainsKey("X-Digipost-Signature"));
        }

        private static HttpClientHandler CreateTestTokenHandler(X509Certificate2 clientCertificate, X509Certificate2 expectedServerCertificate)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, certificate, _, _) => IsExpectedCertificate(certificate, expectedServerCertificate)
            };
            handler.ClientCertificates.Add(clientCertificate);

            return handler;
        }

        private static HttpClientHandler CreateResourceServerHandler(X509Certificate2 expectedServerCertificate)
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, certificate, _, _) => IsExpectedCertificate(certificate, expectedServerCertificate)
            };
        }

        // These self-signed test certificates aren't rooted in anything the OS trust store recognizes, and
        // have no SAN, so default chain/hostname validation would always reject them. Rather than bypassing
        // validation outright, pin against the exact certificate the test itself handed to MtlsTestServer -
        // proving the server presented the certificate we expect, not just "some" certificate.
        private static bool IsExpectedCertificate(X509Certificate2 certificate, X509Certificate2 expectedServerCertificate)
        {
            return certificate != null && certificate.Thumbprint == expectedServerCertificate.Thumbprint;
        }
    }
}
