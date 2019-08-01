using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
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
                var expectedDto = new document
                {
                    subject = source.Subject,
                    filetype = source.FileType,
                    authenticationlevel = source.AuthenticationLevel.ToAuthenticationLevel(),
                    authenticationlevelSpecified = true,
                    sensitivitylevel = source.SensitivityLevel.ToSensitivityLevel(),
                    sensitivitylevelSpecified = true,
                    smsnotification = new smsnotification {afterhours = source.SmsNotification.NotifyAfterHours.ToArray()},
                    uuid = source.Guid
                };

                //Act
                var actualDto = SendDataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void Invoice()
            {
                //Arrange
                var contentBytes = new byte[] {0xb2};
                var smsNotification = new SmsNotification(DateTime.Today.AddHours(3));

                var source = new Invoice(
                    "subject",
                    "txt",
                    contentBytes,
                    100,
                    "8902438456",
                    DateTime.Today,
                    "123123123",
                    AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive,
                    smsNotification);

                var expectedDto = new
                    invoice
                    {
                        subject = source.Subject,
                        filetype = source.FileType,
                        authenticationlevel = source.AuthenticationLevel.ToAuthenticationLevel(),
                        authenticationlevelSpecified = true,
                        sensitivitylevel = source.SensitivityLevel.ToSensitivityLevel(),
                        sensitivitylevelSpecified = true,
                        smsnotification = new smsnotification {at = new[] {new listedtime {time = smsNotification.NotifyAtTimes.First(), timeSpecified = true}}},
                        uuid = source.Guid,
                        kid = source.Kid,
                        amount = source.Amount,
                        account = source.Account,
                        duedate = source.Duedate
                    };

                //Act
                var actualDto = SendDataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void Message()
            {
                //Arrange
                var deliverytime = DateTime.Now.AddDays(3);

                var source = new messagedelivery
                {
                    primarydocument = new document
                    {
                        subject = "TestSubject",
                        filetype = "txt",
                        authenticationlevel = authenticationlevel.TWO_FACTOR,
                        sensitivitylevel = sensitivitylevel.SENSITIVE,
                        uuid = "uuid",
                        contenthash = new contenthash {hashalgorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                    },
                    attachment = new[]
                    {
                        new document
                        {
                            subject = "TestSubject Attachment",
                            filetype = "txt",
                            authenticationlevel = authenticationlevel.TWO_FACTOR,
                            sensitivitylevel = sensitivitylevel.SENSITIVE,
                            uuid = "attachmentGuid",
                            contenthash = new contenthash {hashalgorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                        }
                    },
                    deliverytime = deliverytime,
                    deliverymethod = channel.DIGIPOST,
                    deliverytimeSpecified = true,
                    status = messagestatus.DELIVERED,
                    senderid = 123456
                };

                var expected = new MessageDeliveryResult
                {
                    PrimaryDocument = new Document(source.primarydocument.subject, source.primarydocument.filetype, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive)
                    {
                        Guid = source.primarydocument.uuid,
                        ContentHash = new ContentHash {HashAlgoritm = source.primarydocument.contenthash.hashalgorithm, Value = source.primarydocument.contenthash.Value}
                    },
                    Attachments = new List<Document>
                    {
                        new Document(source.attachment[0].subject, source.attachment[0].filetype, AuthenticationLevel.TwoFactor, SensitivityLevel.Sensitive)
                        {
                            Guid = source.attachment[0].uuid,
                            ContentHash = new ContentHash {HashAlgoritm = source.attachment[0].contenthash.hashalgorithm, Value = source.attachment[0].contenthash.Value}
                        }
                    },
                    DeliveryTime = source.deliverytime,
                    DeliveryMethod = DeliveryMethod.Digipost,
                    Status = MessageStatus.Delivered,
                    SenderId = source.senderid
                };

                //Act
                var actual = SendDataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Comparator.AssertEqual(expected, actual);
            }
        }

        public class ToDataTransferObjectConverterMethod
        {
            [Fact]
            public void Document()
            {
                //Arrange
                var source = new document
                {
                    subject = "testSubject",
                    filetype = "txt",
                    authenticationlevel = authenticationlevel.PASSWORD,
                    sensitivitylevel = sensitivitylevel.SENSITIVE,
                    smsnotification = new smsnotification {afterhours = new[] {3}},
                    uuid = "uuid",
                    contenthash = new contenthash {hashalgorithm = "SHA256", Value = "5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc="}
                };

                IDocument expected = new Document(source.subject, source.filetype, AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3))
                {
                    ContentHash = new ContentHash {HashAlgoritm = source.contenthash.hashalgorithm, Value = source.contenthash.Value},
                    Guid = source.uuid
                };

                //Act
                var actual = SendDataTransferObjectConverter.FromDataTransferObject(source);

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
                var actualDto = SendDataTransferObjectConverter.ToDataTransferObject(source);

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
                expectedDto.recipient.printdetails = DomainUtility.GetPrintDetailsDataTransferObject();

                //Act
                var actualDto = SendDataTransferObjectConverter.ToDataTransferObject(source);

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
                    new message
                    {
                        Item = sender.Id,
                        recipient = new messagerecipient
                        {
                            Item = new nameandaddress
                            {
                                fullname = "Ola Nordmann",
                                postalcode = "0001",
                                city = "Oslo",
                                addressline1 = "Biskop Gunnerus Gate 14"
                            },
                            ItemElementName = ItemChoiceType1.nameandaddress,
                            printdetails = DomainUtility.GetPrintDetailsDataTransferObject()
                        },
                        primarydocument = new document
                        {
                            subject = "PrimaryDocument subject",
                            filetype = "txt",
                            uuid = "primaryDocumentGuid",
                            authenticationlevelSpecified = true,
                            sensitivitylevelSpecified = true
                        },
                        attachment = new[]
                        {
                            new document
                            {
                                subject = "TestSubject attachment subject",
                                filetype = "txt",
                                uuid = "attachmentGuid",
                                sensitivitylevelSpecified = true,
                                authenticationlevelSpecified = true
                            }
                        },
                        deliverytime = DateTime.Today.AddDays(3),
                        deliverytimeSpecified = true
                    };

                //Act
                var actualDto = SendDataTransferObjectConverter.ToDataTransferObject(source);

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
                    new message
                    {
                        Item = sender.Id,
                        recipient = new messagerecipient
                        {
                            Item = new nameandaddress
                            {
                                fullname = "Ola Nordmann",
                                postalcode = "0001",
                                city = "Oslo",
                                addressline1 = "Biskop Gunnerus Gate 14"
                            },
                            ItemElementName = ItemChoiceType1.nameandaddress,
                            printdetails = DomainUtility.GetPrintDetailsDataTransferObject()
                        },
                        primarydocument = new document
                        {
                            subject = "PrimaryDocument subject",
                            filetype = "txt",
                            uuid = "primaryDocumentGuid",
                            authenticationlevelSpecified = true,
                            sensitivitylevelSpecified = true
                        },
                        attachment = new[]
                        {
                            new document
                            {
                                subject = "TestSubject attachment subject",
                                filetype = "txt",
                                uuid = "attachmentGuid",
                                sensitivitylevelSpecified = true,
                                authenticationlevelSpecified = true
                            }
                        },
                        deliverytime = DateTime.Today.AddDays(3),
                        deliverytimeSpecified = true,
                        printifunread = new printifunread
                        {
                            printifunreadafter = deadline,
                            printdetails = DomainUtility.GetPrintDetailsDataTransferObject()
                        }
                    };

                //Act
                var actualDto = SendDataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void SmsNotification()
            {
                //Arrange
                var atTimes = new List<DateTime> {DateTime.Now, DateTime.Now.AddHours(3)};
                var afterHours = new List<int> {4, 5};

                var sourceDto = new smsnotification
                {
                    afterhours = afterHours.ToArray(),
                    at = atTimes.Select(a => new listedtime {timeSpecified = true, time = a}).ToArray()
                };

                var expected = new SmsNotification();
                expected.NotifyAfterHours.AddRange(afterHours);
                expected.NotifyAtTimes.AddRange(atTimes);

                //Act
                var actual = SendDataTransferObjectConverter.FromDataTransferObject(sourceDto);

                //Assert
                Comparator.AssertEqual(expected, actual);
            }
        }
    }
}
