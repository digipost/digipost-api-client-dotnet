using System;
using System.IO;
using System.Reflection;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.SendMessage
{
    [TestClass]
    public class InvoiceTests
    {
        [TestClass]
        public class ConstructorMethod : InvoiceTests
        {
            private const string Subject = "subjcet";
            private const string FileType = "pdf";
            private const decimal Amount = 10;
            private const string Account = "123123";
            private const string Kid = "123123123";
            private const AuthenticationLevel AuthenticationLevel = Domain.Enums.AuthenticationLevel.Password;
            private const SensitivityLevel SensitivityLevel = Domain.Enums.SensitivityLevel.Normal;
            private readonly DateTime _duedate = DateTime.Now;
            private readonly ISmsNotification _smsNotification = new SmsNotification(1);

            [TestMethod]
            public void ConstuctorStreamTest()
            {
                //Arrange
                var contentStream =
                    Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("Digipost.Api.Client.Tests.Resources.Hoveddokument.pdf");
                //Act
                var invoice = new Invoice(Subject, FileType, contentStream, Amount, Account, _duedate, Kid,
                    AuthenticationLevel, SensitivityLevel, _smsNotification);

                //Assert
                Assert.AreEqual(Subject, invoice.Subject);
                Assert.AreEqual(FileType, invoice.FileType);
                Assert.AreEqual(Amount, invoice.Amount);
                Assert.AreEqual(Account, invoice.Account);
                Assert.AreEqual(_duedate, invoice.Duedate);
                Assert.AreEqual(Kid, invoice.Kid);
                Assert.AreEqual(AuthenticationLevel, invoice.AuthenticationLevel);
                Assert.AreEqual(_smsNotification, invoice.SmsNotification);

                var expectedBytes = GetBytesFromEmbeddedResource();
                var sutBytes = invoice.ContentBytes;
                CollectionAssert.AreEqual(expectedBytes, sutBytes);
            }

            [TestMethod]
            public void ConstructorPathTest()
            {
                //Arrange
                var tempFile = CreateTempFile();

                //Act
                var invoice = new Invoice(Subject, FileType, tempFile, Amount, Account, _duedate, Kid,
                    AuthenticationLevel, SensitivityLevel, _smsNotification);
                //Assert
                Assert.AreEqual(Subject, invoice.Subject);
                Assert.AreEqual(FileType, invoice.FileType);
                Assert.AreEqual(Amount, invoice.Amount);
                Assert.AreEqual(Account, invoice.Account);
                Assert.AreEqual(_duedate, invoice.Duedate);
                Assert.AreEqual(Kid, invoice.Kid);
                Assert.AreEqual(AuthenticationLevel, invoice.AuthenticationLevel);
                Assert.AreEqual(_smsNotification, invoice.SmsNotification);

                var expectedBytes = BytesFromFileStream(new FileStream(tempFile, FileMode.Open));
                var sutBytes = invoice.ContentBytes;
                CollectionAssert.AreEqual(expectedBytes, sutBytes);
            }

            [TestMethod]
            public void ConstructorBytesTest()
            {
                //Arrange
                var fileBytes = GetBytesFromEmbeddedResource();

                //Act
                var invoice = new Invoice(Subject, FileType, fileBytes, Amount, Account, _duedate, Kid,
                    AuthenticationLevel, SensitivityLevel, _smsNotification);
                //Assert
                Assert.AreEqual(Subject, invoice.Subject);
                Assert.AreEqual(FileType, invoice.FileType);
                Assert.AreEqual(Amount, invoice.Amount);
                Assert.AreEqual(Account, invoice.Account);
                Assert.AreEqual(_duedate, invoice.Duedate);
                Assert.AreEqual(Kid, invoice.Kid);
                Assert.AreEqual(AuthenticationLevel, invoice.AuthenticationLevel);
                Assert.AreEqual(_smsNotification, invoice.SmsNotification);

                CollectionAssert.AreEqual(fileBytes,invoice.ContentBytes);
            }

            private static string CreateTempFile()
            {
                var tempFile = Path.GetTempFileName();
                using (var fileStream = new FileStream(tempFile, FileMode.Append))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine("testulf testesen");
                        writer.Flush();
                        writer.Close();
                    }
                }
                return tempFile;
            }

            private static byte[] GetBytesFromEmbeddedResource()
            {
                using (
                    var fileStream =
                        Assembly.GetExecutingAssembly()
                            .GetManifestResourceStream("Digipost.Api.Client.Tests.Resources.Hoveddokument.pdf"))
                {
                    return BytesFromFileStream(fileStream);
                }
            }

            private static byte[] BytesFromFileStream(Stream fileStream)
            {
                if (fileStream == null) return null;
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }
    }
}