using System;
using System.Security.Cryptography;
using System.Text;
using Digipost.Api.Client.Common.Handlers;
using Digipost.Api.Client.Resources.Certificate;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Handlers
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
                var computedHash = AuthenticationHandler.ComputeHash(contentBytes);

                //Assert
                var expectedHash = "gvXOB75lBGBY6LVTAVVpapZkBOv531VUE0EHrP2rryE=";

                Assert.Equal(expectedHash, computedHash);
            }
        }
    }
}
