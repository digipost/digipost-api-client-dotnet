using System.IO;
using System.Linq;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.Fakes;
using Xunit;


namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    
    public class DocumentTests
    {
        
        public class ConstructorMethod : DocumentTests
        {
            [Fact]
            public void DocumentFromBytes()
            {
                //Arrange
                Document document = new Document("Subject", "txt", new byte[] { 1, 2, 3}, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Act

                //Assert
                Assert.NotNull(document.Guid);
                Assert.Equal("Subject", document.Subject);
                Assert.Equal("txt", document.FileType);
                Assert.True(document.ContentBytes.SequenceEqual(new byte[] { 1, 2, 3}));
                Assert.Equal(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.Equal(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.Equal(new SmsNotification(2), document.SmsNotification.);
            }

            [Fact]
            public void DocumentFromFile()
            {
                //Arrange
                var document = new FakeDocument("Subject", "txt", "c://imaginary/file", AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Act

                //Assert
                Assert.NotNull(document.Guid);
                Assert.Equal("Subject", document.Subject);
                Assert.Equal("txt", document.FileType);
                Assert.True(document.ContentBytes.SequenceEqual(new byte[]{1,2,3,4}));
                Assert.Equal(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.Equal(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.Equal(new SmsNotification(2), document.SmsNotification);
            }

            [Fact]
            public void DocumentFromStream()
            {
                //Arrange
                byte[] myByteArray = { 1, 2, 3};
                var stream = new MemoryStream(myByteArray);
                Document document = new FakeDocument("Subject", "txt", stream , AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Act

                //Assert
                Assert.NotNull(document.Guid);
                Assert.Equal("Subject", document.Subject);
                Assert.Equal("txt", document.FileType);
                Assert.True(document.ContentBytes.SequenceEqual(new byte[] { 1, 2, 3 }));
                Assert.Equal(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.Equal(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.Equal(new SmsNotification(2), document.SmsNotification);
            } 


        }
    }
}
