using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Send.Tests
{
    public class SendDataTransferObjectConverterTests
    {
        private static readonly Comparator _comparator = new Comparator();

        public class FromDataTransferObjectConverterMethod
        {
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
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);

                Assert.Equal(0, differences.Count());
            }
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

            IEnumerable<IDifference> differences;
            _comparator.Equal(expectedDto, actualDto, out differences);
            Assert.Equal(0, differences.Count());
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

            IEnumerable<IDifference> differences;
            _comparator.Equal(expectedDto, actualDto, out differences);
            Assert.Equal(0, differences.Count());
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

            IEnumerable<IDifference> differences;
            _comparator.Equal(expectedDto, actualDto, out differences);
            Assert.Equal(0, differences.Count());
        }
    }
}