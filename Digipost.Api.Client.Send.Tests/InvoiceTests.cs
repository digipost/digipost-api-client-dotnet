﻿using System;
using System.IO;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Resources.Content;
using Xunit;

namespace Digipost.Api.Client.Send.Tests
{
    public class InvoiceTests
    {
        public class ConstructorMethod : InvoiceTests
        {
            private const string Subject = "subjcet";
            private const string FileType = "pdf";
            private const decimal Amount = 10;
            private const string Account = "123123";
            private const string Kid = "123123123";
            private const AuthenticationLevel AuthenticationLevel = Common.Enums.AuthenticationLevel.Password;
            private const SensitivityLevel SensitivityLevel = Common.Enums.SensitivityLevel.Normal;
            private readonly DateTime _duedate = DateTime.Now;
            private readonly ISmsNotification _smsNotification = new SmsNotification(1);

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

            private static byte[] BytesFromFileStream(Stream fileStream)
            {
                if (fileStream == null) return null;
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            }

            [Fact]
            public void ConstructorBytesTest()
            {
                //Arrange
                var fileBytes = ContentResource.Hoveddokument.Pdf();

                //Act
                var invoice = new Invoice(Subject, FileType, fileBytes, Amount, Account, _duedate, Kid,
                    AuthenticationLevel, SensitivityLevel, _smsNotification);
                //Assert
                Assert.Equal(Subject, invoice.Subject);
                Assert.Equal(FileType, invoice.FileType);
                Assert.Equal(Amount, invoice.Amount);
                Assert.Equal(Account, invoice.Account);
                Assert.Equal(_duedate, invoice.Duedate);
                Assert.Equal(Kid, invoice.Kid);
                Assert.Equal(AuthenticationLevel, invoice.AuthenticationLevel);
                Assert.Equal(_smsNotification, invoice.SmsNotification);

                Assert.Equal(fileBytes, invoice.ContentBytes);
            }

            [Fact]
            public void ConstructorPathTest()
            {
                //Arrange
                var tempFile = CreateTempFile();

                //Act
                var invoice = new Invoice(Subject, FileType, tempFile, Amount, Account, _duedate, Kid, AuthenticationLevel, SensitivityLevel, _smsNotification);
                //Assert
                Assert.Equal(Subject, invoice.Subject);
                Assert.Equal(FileType, invoice.FileType);
                Assert.Equal(Amount, invoice.Amount);
                Assert.Equal(Account, invoice.Account);
                Assert.Equal(_duedate, invoice.Duedate);
                Assert.Equal(Kid, invoice.Kid);
                Assert.Equal(AuthenticationLevel, invoice.AuthenticationLevel);
                Assert.Equal(_smsNotification, invoice.SmsNotification);

                var expectedBytes = BytesFromFileStream(new FileStream(tempFile, FileMode.Open));
                Assert.Equal(expectedBytes, invoice.ContentBytes);
            }

            [Fact]
            public void ConstuctorStreamTest()
            {
                //Arrange
                var contentStream = new MemoryStream(ContentResource.Hoveddokument.Pdf());

                //Act
                var invoice = new Invoice(Subject, FileType, contentStream, Amount, Account, _duedate, Kid, AuthenticationLevel, SensitivityLevel, _smsNotification);

                //Assert
                Assert.Equal(Subject, invoice.Subject);
                Assert.Equal(FileType, invoice.FileType);
                Assert.Equal(Amount, invoice.Amount);
                Assert.Equal(Account, invoice.Account);
                Assert.Equal(_duedate, invoice.Duedate);
                Assert.Equal(Kid, invoice.Kid);
                Assert.Equal(AuthenticationLevel, invoice.AuthenticationLevel);
                Assert.Equal(_smsNotification, invoice.SmsNotification);
                Assert.NotNull(invoice.ContentBytes);
            }
        }
    }
}
