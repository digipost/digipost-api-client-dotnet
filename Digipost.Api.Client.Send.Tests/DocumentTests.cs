using System.IO;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Resources.Content;
using Xunit;

namespace Digipost.Api.Client.Send.Tests
{
    public class DocumentTests
    {
        public class ConstructorMethod : DocumentTests
        {
            [Fact]
            public void DocumentFromBytes()
            {
                //Arrange
                var document = new Document("Subject", "txt", new byte[] {1, 2, 3}, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Act

                //Assert
                Assert.NotNull(document.Guid);
                Assert.Equal("Subject", document.Subject);
                Assert.Equal("txt", document.FileType);
                Assert.True(document.ContentBytes.SequenceEqual(new byte[] {1, 2, 3}));
                Assert.Equal(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.Equal(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.Equal(new SmsNotification(2), document.SmsNotification.);
            }

            [Fact]
            public void DocumentFromFilePath()
            {
                //Arrange
                var tempFile = CreateTempFile();

                //Act
                var document = new Document("Subject", "txt", tempFile, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Assert
                Assert.NotNull(document.Guid);
                Assert.Equal("Subject", document.Subject);
                Assert.Equal("txt", document.FileType);
                var expectedBytes = BytesFromStream(new FileStream(tempFile, FileMode.Open));
                Assert.Equal(expectedBytes, document.ContentBytes);
                Assert.Equal(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.Equal(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.Equal(new SmsNotification(2), document.SmsNotification);
            }

            [Fact]
            public void DocumentFromStream()
            {
                //Arrange
                var contentStream = new MemoryStream(ContentResource.Hoveddokument.Pdf());

                //Act
                Document document = new Document("Subject", "txt", contentStream, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive, new SmsNotification(2));

                //Assert
                Assert.NotNull(document.Guid);
                Assert.Equal("Subject", document.Subject);
                Assert.Equal("txt", document.FileType);
                var expectedBytes = BytesFromStream(new MemoryStream(ContentResource.Hoveddokument.Pdf()));
                Assert.Equal(expectedBytes, document.ContentBytes);
                Assert.Equal(AuthenticationLevel.TwoFactor, document.AuthenticationLevel);
                Assert.Equal(SensitivityLevel.Sensitive, document.SensitivityLevel);
                //Assert.Equal(new SmsNotification(2), document.SmsNotification);
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

            private static byte[] BytesFromStream(Stream fileStream)
            {
                if (fileStream == null) return null;
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }
    }
}
