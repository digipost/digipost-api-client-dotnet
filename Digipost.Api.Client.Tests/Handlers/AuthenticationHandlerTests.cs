using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Handlers
{
    public class AuthenticationHandlerTests
    {
        public class ComputeSignatureMethod
        {
            [Fact]
            public void ReturnsCorrectSignature()
            {
                //Arrange
                const string senderId = "1337";
                const string uri = "http://fakeuri.no/someendpoint";
                const string method = "POST";
                const string sha256Hash = "TheHashOfContentForHeader";
                const string expectedSignature =
                    "HEZfhL+mu0Pb9Owvfs7pHLUXxZPthONK53nWTwXPFtFVjslr4AIxLqUSbAO7PerzBcRryYa84SellVabx8t16Ixg52afLQb02qyeDx1qF23YAIvvv01NmEJkVUUTV/oN7MgDAb4NGeujzVoUzXKTV+b5YC4W2c4M/RWSGYF1HxEEo+82SDyTlwGa3XxhcVem2Kg0LOgZvKaJnFWk0fsVDI7J9xWdOY0NWbtlm/xu77w2IlR+91lbr2G5A75lyzboXVEYvOj3UGzKwFTqGDpR7var+/PzWh00lQ/dKtILKzDGz3E80CxCOtlU/6kczk9MtYVQvLCy7QR0GMUI6ypTzg==";
                var certificate = CertificateResource.Certificate();
                var dateTime = new DateTime(2014, 07, 07, 12, 00, 02).ToString("R");

                //Act
                var computedSignature = AuthenticationHandler.ComputeSignature(method, new Uri(uri), dateTime, sha256Hash,
                    senderId,
                    certificate,
                    false);

                //Assert
                Assert.Equal(expectedSignature, computedSignature);
            }
        }

        public class ComputeHashMethod
        {
            [Fact]
            public void ReturnsCorrectHash()
            {
                //Arrange
                var contentBytes = Encoding.UTF8.GetBytes("This is the content to hash.");

                //Act
                var computedHash = RequestHeaderUtility.ComputeContentHash(contentBytes);

                //Assert
                var expectedHash = "gvXOB75lBGBY6LVTAVVpapZkBOv531VUE0EHrP2rryE=";

                Assert.Equal(expectedHash, computedHash);
            }
        }

        public class SendAsyncMethod
        {
            [Fact]
            public async Task SetsExpectedHeaders()
            {
                var clientConfig = new ClientConfig(new Broker(1337), Environment.Test);
                var certificate = CertificateResource.Certificate();

                var handler = new AuthenticationHandler(clientConfig, certificate, new NullLoggerFactory())
                {
                    InnerHandler = new FakeResponseHandler {ResultCode = HttpStatusCode.OK, HttpContent = new StringContent(string.Empty)}
                };
                var invoker = new HttpMessageInvoker(handler);

                var request = new HttpRequestMessage(HttpMethod.Post, "http://fakeuri.no/someendpoint")
                {
                    Content = new StringContent("body")
                };

                await invoker.SendAsync(request, CancellationToken.None);

                Assert.True(request.Headers.Contains("X-Digipost-UserId"));
                Assert.True(request.Headers.Contains("Date"));
                Assert.True(request.Headers.Contains("Accept"));
                Assert.True(request.Headers.Contains("User-Agent"));
                Assert.True(request.Headers.Contains("X-Content-SHA256"));
                Assert.True(request.Headers.Contains("X-Digipost-Signature"));
            }
        }
    }
}
