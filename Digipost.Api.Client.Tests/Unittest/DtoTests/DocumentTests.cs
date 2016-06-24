using System.IO;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class DocumentTests
    {
        [TestClass]
        public class ConstructorMethod : DocumentTests
        {
            [TestMethod]
            public void DocumentFromBytes()
            {
                //Arrange
                var document = new Document("Subject", "txt", new byte[] {1, 2, 3}, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Act

                //Assert
                Assert.IsNotNull(document.Guid);
                Assert.AreEqual("Subject", document.Subject);
                Assert.AreEqual("txt", document.FileType);
                Assert.IsTrue(document.ContentBytes.SequenceEqual(new byte[] {1, 2, 3}));
                Assert.AreEqual(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.AreEqual(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.AreEqual(new SmsNotification(2), document.SmsNotification.);
            }

            [TestMethod]
            public void DocumentFromFile()
            {
                //Arrange
                var document = new FakeDocument("Subject", "txt", "c://imaginary/file", AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Act

                //Assert
                Assert.IsNotNull(document.Guid);
                Assert.AreEqual("Subject", document.Subject);
                Assert.AreEqual("txt", document.FileType);
                Assert.IsTrue(document.ContentBytes.SequenceEqual(new byte[] {1, 2, 3, 4}));
                Assert.AreEqual(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.AreEqual(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.AreEqual(new SmsNotification(2), document.SmsNotification);
            }

            [TestMethod]
            public void DocumentFromStream()
            {
                //Arrange
                byte[] myByteArray = {1, 2, 3};
                var stream = new MemoryStream(myByteArray);
                Document document = new FakeDocument("Subject", "txt", stream, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Act

                //Assert
                Assert.IsNotNull(document.Guid);
                Assert.AreEqual("Subject", document.Subject);
                Assert.AreEqual("txt", document.FileType);
                Assert.IsTrue(document.ContentBytes.SequenceEqual(new byte[] {1, 2, 3}));
                Assert.AreEqual(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.AreEqual(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.AreEqual(new SmsNotification(2), document.SmsNotification);
            }
        }
    }
}