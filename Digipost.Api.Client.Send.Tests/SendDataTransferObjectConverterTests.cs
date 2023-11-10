using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
using V8 = Digipost.Api.Client.Common.Generated.V8;
using Xunit;

namespace Digipost.Api.Client.Send.Tests
{
    public class SendDataTransferObjectConverterTests
    {
        public class FromDataTransferObjectConverterMethod
        {
            [Fact]
            public void Document()
            {
                //Arrange
                IDocument source = new Document("TestSubject", "txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3));
                var expectedDto = new V8.Document
                {
                    Subject = source.Subject,
                    FileType = source.FileType,
                    AuthenticationLevel = source.AuthenticationLevel.ToAuthenticationLevel(),
                    AuthenticationLevelSpecified = true,
                    SensitivityLevel = source.SensitivityLevel.ToSensitivityLevel(),
                    SensitivityLevelSpecified = true,
                    SmsNotification = new V8.SmsNotification {AfterHours = {source.SmsNotification.NotifyAfterHours.ToArray()[0]}},
                    Uuid = source.Guid
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void Message()
            {
                //Arrange
                var deliverytime = DateTime.Now.AddDays(3);

                var source = new V8.MessageDelivery()
                {
                    PrimaryDocument = new V8.Document
                    {
                        Subject = "TestSubject",
                        FileType = "txt",
                        AuthenticationLevel = V8.AuthenticationLevel.TwoFactor,
                        SensitivityLevel = V8.SensitivityLevel.Sensitive,
                        Uuid = "uuid",
                        ContentHash = new V8.ContentHash() {HashAlgorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                    },
                    Attachment = {
                        new V8.Document
                        {
                            Subject = "TestSubject Attachment",
                            FileType = "txt",
                            AuthenticationLevel = V8.AuthenticationLevel.TwoFactor,
                            SensitivityLevel = V8.SensitivityLevel.Sensitive,
                            Uuid = "attachmentGuid",
                            ContentHash = new V8.ContentHash() {HashAlgorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                        }
                    },
                    DeliveryTime = deliverytime,
                    DeliveryMethod = V8.Channel.Digipost,
                    DeliveryTimeSpecified = true,
                    Status = V8.MessageStatus.Delivered,
                    SenderId = 123456
                };

                var expected = new MessageDeliveryResult
                {
                    PrimaryDocument = new Document(source.PrimaryDocument.Subject, source.PrimaryDocument.FileType, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive)
                    {
                        Guid = source.PrimaryDocument.Uuid,
                        ContentHash = new ContentHash {HashAlgoritm = source.PrimaryDocument.ContentHash.HashAlgorithm, Value = source.PrimaryDocument.ContentHash.Value}
                    },
                    Attachments = new List<Document>
                    {
                        new Document(source.Attachment[0].Subject, source.Attachment[0].FileType, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive)
                        {
                            Guid = source.Attachment[0].Uuid,
                            ContentHash = new ContentHash {HashAlgoritm = source.Attachment[0].ContentHash.HashAlgorithm, Value = source.Attachment[0].ContentHash.Value}
                        }
                    },
                    DeliveryTime = source.DeliveryTime,
                    DeliveryMethod = DeliveryMethod.Digipost,
                    Status = MessageStatus.Delivered,
                    SenderId = source.SenderId
                };

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void SmsNotification()
            {
                //Arrange
                var atTimes = new List<DateTime> {DateTime.Now, DateTime.Now.AddHours(3)};
                var afterHours = new List<int> {4, 5};

                var source = new SmsNotification();
                source.NotifyAfterHours.AddRange(afterHours);
                source.NotifyAtTimes.AddRange(atTimes);

                var expectedDto = new V8.SmsNotification();

                afterHours.ForEach(s => expectedDto.AfterHours.Add(s));

                atTimes.Select(a => new V8.ListedTime() {TimeSpecified = true, Time = a})
                    .ToList()
                    .ForEach(time => expectedDto.At.Add(time));

                //Act
                var actual = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actual);
            }
        }

        public class ToDataTransferObjectConverterMethod
        {
            [Fact]
            public void Document()
            {
                //Arrange
                var source = new V8.Document
                {
                    Subject = "testSubject",
                    FileType = "txt",
                    AuthenticationLevel = V8.AuthenticationLevel.Password,
                    SensitivityLevel = V8.SensitivityLevel.Sensitive,
                    SmsNotification = new V8.SmsNotification() {AfterHours = { 3 }},
                    Uuid = "uuid",
                    ContentHash = new V8.ContentHash() {HashAlgorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                };

                IDocument expected = new Document(source.Subject, source.FileType, AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3))
                {
                    ContentHash = new ContentHash {HashAlgoritm = source.ContentHash.HashAlgorithm, Value = source.ContentHash.Value},
                    Guid = source.Uuid
                };

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void Message()
            {
                //Arrange
                var source = DomainUtility.GetMessageWithBytesAndStaticGuidRecipientById();

                var expectedDto = DomainUtility.GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById();

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void MessageWithPrintDetailsAndRecipientById()
            {
                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var source = DomainUtility.GetMessageWithBytesAndStaticGuidRecipientById();
                source.PrintDetails = printDetails;

                var expectedDto = DomainUtility.GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById();
                expectedDto.Recipient.PrintDetails = DomainUtility.GetPrintDetailsDataTransferObject();

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void MessageWithPrintDetailsAndRecipientByNameAndAddress()
            {
                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var sender = new Sender(1010);
                var source = new Message(
                    sender,
                    DomainUtility.GetRecipientByNameAndAddress(),
                    new Document("PrimaryDocument subject", "txt", new byte[3])
                )
                {
                    Attachments = new List<IDocument>
                    {
                        new Document("TestSubject attachment subject", "txt", new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                    DeliveryTime = DateTime.Today.AddDays(3),
                    PrimaryDocument = {Guid = "primaryDocumentGuid"},
                    PrintDetails = printDetails
                };

                var expectedDto =
                    new V8.Message()
                    {
                        SenderId = sender.Id,
                        SenderIdSpecified = true,
                        Recipient = new V8.MessageRecipient()
                        {
                            NameAndAddress = new V8.NameAndAddress()
                            {
                                Fullname = "Ola Nordmann",
                                Postalcode = "0001",
                                City = "Oslo",
                                Addressline1 = "Biskop Gunnerus Gate 14"
                            },
                            PrintDetails = DomainUtility.GetPrintDetailsDataTransferObject()
                        },
                        PrimaryDocument = new V8.Document
                        {
                            Subject = "PrimaryDocument subject",
                            FileType = "txt",
                            Uuid = "primaryDocumentGuid",
                            AuthenticationLevelSpecified = true,
                            SensitivityLevelSpecified = true
                        },
                        Attachment = {
                            new V8.Document
                            {
                                Subject = "TestSubject attachment subject",
                                FileType = "txt",
                                Uuid = "attachmentGuid",
                                SensitivityLevelSpecified = true,
                                AuthenticationLevelSpecified = true
                            }
                        },
                        DeliveryTime = DateTime.Today.AddDays(3),
                        DeliveryTimeSpecified = true
                    };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void MessageWithPrintIfUnread()
            {
                                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var sender = new Sender(1010);
                var deadline = DateTime.Now.AddDays(6);
                var source = new Message(
                    sender,
                    DomainUtility.GetRecipientByNameAndAddress(),
                    new Document("PrimaryDocument subject", "txt", new byte[3])
                )
                {
                    Attachments = new List<IDocument>
                    {
                        new Document("TestSubject attachment subject", "txt", new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                    DeliveryTime = DateTime.Today.AddDays(3),
                    PrimaryDocument = {Guid = "primaryDocumentGuid"},
                    PrintDetails = printDetails,
                    PrintIfUnread = new PrintIfUnread(deadline, printDetails)
                };

                var expectedDto =
                    new V8.Message()
                    {
                        SenderId = sender.Id,
                        SenderIdSpecified = true,
                        Recipient = new V8.MessageRecipient()
                        {
                            NameAndAddress = new V8.NameAndAddress()
                            {
                                Fullname = "Ola Nordmann",
                                Postalcode = "0001",
                                City = "Oslo",
                                Addressline1 = "Biskop Gunnerus Gate 14"
                            },
                            PrintDetails = DomainUtility.GetPrintDetailsDataTransferObject()
                        },
                        PrimaryDocument = new V8.Document
                        {
                            Subject = "PrimaryDocument subject",
                            FileType = "txt",
                            Uuid = "primaryDocumentGuid",
                            AuthenticationLevelSpecified = true,
                            SensitivityLevelSpecified = true
                        },
                        Attachment = {
                            new V8.Document
                            {
                                Subject = "TestSubject attachment subject",
                                FileType = "txt",
                                Uuid = "attachmentGuid",
                                SensitivityLevelSpecified = true,
                                AuthenticationLevelSpecified = true
                            }
                        },
                        DeliveryTime = DateTime.Today.AddDays(3),
                        DeliveryTimeSpecified = true,
                        PrintIfUnread = new V8.PrintIfUnread()
                        {
                            PrintIfUnreadAfter = deadline,
                            PrintDetails = DomainUtility.GetPrintDetailsDataTransferObject()
                        }
                    };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void SmsNotification()
            {
                //Arrange
                var atTimes = new List<DateTime> {DateTime.Now, DateTime.Now.AddHours(3)};
                var afterHours = new List<int> {4, 5};

                var sourceDto = new V8.SmsNotification()
                {
                    AfterHours = { 4, 5 },
                };
                atTimes.Select(a => new V8.ListedTime() {TimeSpecified = true, Time = a})
                    .ToList().ForEach(a => sourceDto.At.Add(a));

                var expected = new SmsNotification();
                expected.NotifyAfterHours.AddRange(afterHours);
                expected.NotifyAtTimes.AddRange(atTimes);

                //Act
                var actual = sourceDto.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }
        }
    }
}
