using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
using V8;
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
                    File_Type = source.FileType,
                    Authentication_Level = source.AuthenticationLevel.ToAuthenticationLevel(),
                    Authentication_LevelSpecified = true,
                    Sensitivity_Level = source.SensitivityLevel.ToSensitivityLevel(),
                    Sensitivity_LevelSpecified = true,
                    Sms_Notification = new Sms_Notification {After_Hours = {source.SmsNotification.NotifyAfterHours.ToArray()[0]}},
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

                var source = new V8.Message_Delivery()
                {
                    Primary_Document = new V8.Document
                    {
                        Subject = "TestSubject",
                        File_Type = "txt",
                        Authentication_Level = Authentication_Level.TWO_FACTOR,
                        Sensitivity_Level = Sensitivity_Level.SENSITIVE,
                        Uuid = "uuid",
                        Content_Hash = new Content_Hash() {Hash_Algorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                    },
                    Attachment = {
                        new V8.Document
                        {
                            Subject = "TestSubject Attachment",
                            File_Type = "txt",
                            Authentication_Level = Authentication_Level.TWO_FACTOR,
                            Sensitivity_Level = Sensitivity_Level.SENSITIVE,
                            Uuid = "attachmentGuid",
                            Content_Hash = new Content_Hash() {Hash_Algorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                        }
                    },
                    Delivery_Time = deliverytime,
                    Delivery_Method = V8.Channel.DIGIPOST,
                    Delivery_TimeSpecified = true,
                    Status = Message_Status.DELIVERED,
                    Sender_Id = 123456
                };

                var expected = new MessageDeliveryResult
                {
                    PrimaryDocument = new Document(source.Primary_Document.Subject, source.Primary_Document.File_Type, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive)
                    {
                        Guid = source.Primary_Document.Uuid,
                        ContentHash = new ContentHash {HashAlgoritm = source.Primary_Document.Content_Hash.Hash_Algorithm, Value = source.Primary_Document.Content_Hash.Value}
                    },
                    Attachments = new List<Document>
                    {
                        new Document(source.Attachment[0].Subject, source.Attachment[0].File_Type, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive)
                        {
                            Guid = source.Attachment[0].Uuid,
                            ContentHash = new ContentHash {HashAlgoritm = source.Attachment[0].Content_Hash.Hash_Algorithm, Value = source.Attachment[0].Content_Hash.Value}
                        }
                    },
                    DeliveryTime = source.Delivery_Time,
                    DeliveryMethod = DeliveryMethod.Digipost,
                    Status = MessageStatus.Delivered,
                    SenderId = source.Sender_Id
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

                var expectedDto = new V8.Sms_Notification();

                afterHours.ForEach(s => expectedDto.After_Hours.Add(s));

                atTimes.Select(a => new V8.Listed_Time() {TimeSpecified = true, Time = a})
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
                    File_Type = "txt",
                    Authentication_Level = Authentication_Level.PASSWORD,
                    Sensitivity_Level = Sensitivity_Level.SENSITIVE,
                    Sms_Notification = new Sms_Notification() {After_Hours = { 3 }},
                    Uuid = "uuid",
                    Content_Hash = new Content_Hash() {Hash_Algorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                };

                IDocument expected = new Document(source.Subject, source.File_Type, AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3))
                {
                    ContentHash = new ContentHash {HashAlgoritm = source.Content_Hash.Hash_Algorithm, Value = source.Content_Hash.Value},
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
                expectedDto.Recipient.Print_Details = DomainUtility.GetPrintDetailsDataTransferObject();

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
                        Sender_Id = sender.Id,
                        Sender_IdSpecified = true,
                        Recipient = new Message_Recipient()
                        {
                            Name_And_Address = new Name_And_Address()
                            {
                                Fullname = "Ola Nordmann",
                                Postalcode = "0001",
                                City = "Oslo",
                                Addressline1 = "Biskop Gunnerus Gate 14"
                            },
                            Print_Details = DomainUtility.GetPrintDetailsDataTransferObject()
                        },
                        Primary_Document = new V8.Document
                        {
                            Subject = "PrimaryDocument subject",
                            File_Type = "txt",
                            Uuid = "primaryDocumentGuid",
                            Authentication_LevelSpecified = true,
                            Sensitivity_LevelSpecified = true
                        },
                        Attachment = {
                            new V8.Document
                            {
                                Subject = "TestSubject attachment subject",
                                File_Type = "txt",
                                Uuid = "attachmentGuid",
                                Sensitivity_LevelSpecified = true,
                                Authentication_LevelSpecified = true
                            }
                        },
                        Delivery_Time = DateTime.Today.AddDays(3),
                        Delivery_TimeSpecified = true
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
                        Sender_Id = sender.Id,
                        Sender_IdSpecified = true,
                        Recipient = new V8.Message_Recipient()
                        {
                            Name_And_Address = new V8.Name_And_Address()
                            {
                                Fullname = "Ola Nordmann",
                                Postalcode = "0001",
                                City = "Oslo",
                                Addressline1 = "Biskop Gunnerus Gate 14"
                            },
                            Print_Details = DomainUtility.GetPrintDetailsDataTransferObject()
                        },
                        Primary_Document = new V8.Document
                        {
                            Subject = "PrimaryDocument subject",
                            File_Type = "txt",
                            Uuid = "primaryDocumentGuid",
                            Authentication_LevelSpecified = true,
                            Sensitivity_LevelSpecified = true
                        },
                        Attachment = {
                            new V8.Document
                            {
                                Subject = "TestSubject attachment subject",
                                File_Type = "txt",
                                Uuid = "attachmentGuid",
                                Sensitivity_LevelSpecified = true,
                                Authentication_LevelSpecified = true
                            }
                        },
                        Delivery_Time = DateTime.Today.AddDays(3),
                        Delivery_TimeSpecified = true,
                        Print_If_Unread = new Print_If_Unread()
                        {
                            Print_If_Unread_After = deadline,
                            Print_Details = DomainUtility.GetPrintDetailsDataTransferObject()
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

                var sourceDto = new Sms_Notification()
                {
                    After_Hours = { 4, 5 },
                };
                atTimes.Select(a => new Listed_Time() {TimeSpecified = true, Time = a})
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
