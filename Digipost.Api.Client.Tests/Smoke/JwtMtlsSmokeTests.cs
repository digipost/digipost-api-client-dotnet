using System.Text;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Send;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Smoke
{
    // Manual smoke test proving JWT/mTLS authentication works end-to-end against a real Digipost Test
    // environment. Fill in the placeholders below before running.
    public class JwtMtlsSmokeTests
    {
        private readonly ITestOutputHelper _output;

        // TODO: fill in with your real Test-environment credentials before running. Supply your own
        // certificate, installed in your OS certificate store (see the enterprise certificate setup docs) -
        // none is checked into this repo.
        private const long BrokerId = 0; // Your broker id in Environment.Test.
        private const string ClientId = "your-oauth-client-id";
        private const string Thumbprint = "your-certificate-thumbprint";

        // TODO: fill in with a recipient personal identification number that has an active Digipost account.
        private const string RecipientPersonalIdentificationNumber = "TODO";

        public JwtMtlsSmokeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            return serviceProvider.GetService<ILoggerFactory>();
        }

        private static ClientConfig CreateClientConfig()
        {
            return new ClientConfig(new Broker(BrokerId), Environment.Test)
            {
                LogRequestAndResponse = true,
                TimeoutMilliseconds = 60000
            };
        }

        private static JwtAuthConfig CreateJwtAuthConfig()
        {
            return new JwtAuthConfig(ClientId, thumbprint: Thumbprint);
        }

        private static DigipostClient CreateClient()
        {
            return new DigipostClient(CreateClientConfig(), CreateJwtAuthConfig(), CreateLoggerFactory());
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_fetch_token_only_with_jwt_mtls_authentication()
        {
            _output.WriteLine("Starting smoke test: Can_fetch_token_only_with_jwt_mtls_authentication");

            var clientConfig = CreateClientConfig();
            var jwtAuthConfig = CreateJwtAuthConfig();
            var tokenProvider = new TokenProvider(clientConfig, jwtAuthConfig, CreateLoggerFactory());

            var token = tokenProvider.GetTokenAsync().GetAwaiter().GetResult();

            _output.WriteLine($"Got token (first 12 chars): {token.Substring(0, System.Math.Min(12, token.Length))}...");
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_fetch_entrypoint_with_jwt_mtls_authentication()
        {
            _output.WriteLine("Starting smoke test: Can_fetch_entrypoint_with_jwt_mtls_authentication");
            var client = CreateClient();

            var root = client.GetRoot(new ApiRootUri());
            _output.WriteLine($"Root: {root}");

            Assert.NotNull(root);
            _output.WriteLine("Smoke test completed: API root fetched successfully.");
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_get_sender_information_with_jwt_mtls_authentication()
        {
            _output.WriteLine("Starting smoke test: Can_get_sender_information_with_jwt_mtls_authentication");
            var client = CreateClient();

            var senderInformation = client.GetSenderInformation();

            Assert.True(senderInformation.IsValidSender);
            _output.WriteLine($"Smoke test completed: IsValidSender={senderInformation.IsValidSender}.");
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_send_message_with_jwt_mtls_authentication()
        {
            _output.WriteLine("Starting smoke test: Can_send_message_with_jwt_mtls_authentication");
            var client = CreateClient();

            var sender = new Sender(BrokerId);
            var recipient = new RecipientById(IdentificationType.PersonalIdentificationNumber, RecipientPersonalIdentificationNumber);
            var document = new Document(
                subject: "JWT/mTLS smoke test",
                fileType: "txt",
                contentBytes: Encoding.UTF8.GetBytes("Sent via JWT/mTLS authentication smoke test.")
            );

            var message = new Message(sender, recipient, document);

            var result = client.SendMessage(message);

            _output.WriteLine($"Smoke test completed: MessageId={result.MessageId}, Status={result.Status}, DeliveryMethod={result.DeliveryMethod}.");
            Assert.NotNull(result);
        }
    }
}
