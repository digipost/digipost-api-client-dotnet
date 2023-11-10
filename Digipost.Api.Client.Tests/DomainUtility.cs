using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.DataTypes.Core;
using Digipost.Api.Client.Resources.Content;
using Digipost.Api.Client.Send;
using V8 = Digipost.Api.Client.Common.Generated.V8;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests
{
    public static class DomainUtility
    {
        public static ClientConfig GetClientConfig()
        {
            return new ClientConfig(new Broker(1010), Environment.Production);
        }

        public static Sender GetSender()
        {
            return new Sender(1010);
        }

        public static IMessage GetSimpleMessageWithRecipientById()
        {
            return GetSimpleMessageWithRecipientById(GetDocument());
        }

        public static IMessage GetSimpleMessageWithRecipientById(IDocument document)
        {
            var message = new Send.Message(
                GetSender(),
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"),
                document
            );
            return message;
        }

        public static IMessage GetMessageWithBytesAndStaticGuidRecipientById()
        {
            var deliverytime = DateTime.Today.AddDays(3);
            var recipientById = GetRecipientByDigipostId();

            return new Send.Message(GetSender(), recipientById, new Document("TestSubject", "txt", new byte[3]))
            {
                Id = "ThatMessageId",
                Attachments = new List<IDocument>
                {
                    new Document("TestSubject attachment", "txt", new byte[3])
                    {
                        Guid = "attachmentGuid"
                    }
                },
                DeliveryTime = deliverytime,
                PrimaryDocument = {Guid = "attachmentGuidPrimary"}
            };
        }

        public static V8.Message GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById()
        {
            var message = new V8.Message()
            {
                SenderId = long.Parse("1010"),
                SenderIdSpecified = true,
                MessageId = "ThatMessageId",
                PrimaryDocument = new V8.Document()
                {
                    Subject = "TestSubject",
                    FileType = "txt",
                    Uuid = "attachmentGuidPrimary",
                    AuthenticationLevelSpecified = true,
                    SensitivityLevelSpecified = true
                },
                DeliveryTime = DateTime.Today.AddDays(3),
                DeliveryTimeSpecified = true,
                Recipient = new V8.MessageRecipient()
                {
                    DigipostAddress = "ola.nordmann#246BB"
                }
            };
            message.Attachment.Add(new V8.Document()
            {
                Subject = "TestSubject attachment",
                FileType = "txt",
                Uuid = "attachmentGuid",
                AuthenticationLevelSpecified = true,
                SensitivityLevelSpecified = true
            });

            return message;
        }

        public static IMessage GetSimpleMessageWithRecipientByNameAndAddress()
        {
            return new Send.Message(GetSender(), GetRecipientByNameAndAddress(), GetDocument());
        }

        public static IDocument GetDocument(IDigipostDataType dataType = null)
        {
            return new Document("simple-document-dotnet", "pdf", ContentResource.Hoveddokument.Pdf())
            {
                DataType = dataType
            };
        }

        public static IIdentification GetPersonalIdentification()
        {
            var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"));
            return identification;
        }

        public static RecipientById GetRecipientByDigipostId()
        {
            return new RecipientById(IdentificationType.DigipostAddress, "ola.nordmann#246BB");
        }

        public static RecipientByNameAndAddress GetRecipientByNameAndAddress()
        {
            return new RecipientByNameAndAddress("Ola Nordmann",
                postalCode: "0001",
                city: "Oslo",
                addressLine1: "Biskop Gunnerus Gate 14"
            );
        }

        public static PrintDetails GetPrintDetails()
        {
            return
                new PrintDetails(
                    new PrintRecipient("Ola Nordmann", new NorwegianAddress("0115", "Oslo", "Osloveien 15")),
                    new PrintReturnRecipient("Returkongen",
                        new NorwegianAddress("5510", "Sophaugen", "Sophauggata 22")));
        }

        public static V8.PrintDetails GetPrintDetailsDataTransferObject()
        {
            return new V8.PrintDetails
            {
                Recipient = new V8.PrintRecipient()
                {
                    Name = "Ola Nordmann",
                    NorwegianAddress = new V8.NorwegianAddress
                    {
                        Addressline1 = "Osloveien 15",
                        City = "Oslo",
                        ZipCode = "0115"
                    }
                },
                ReturnAddress = new V8.PrintRecipient
                {
                    Name = "Returkongen",
                    NorwegianAddress = new V8.NorwegianAddress()
                    {
                        Addressline1 = "Sophauggata 22",
                        City = "Sophaugen",
                        ZipCode = "5510"
                    }
                }
            };
        }

        public static PrintIfUnread GetPrintIfUnread()
        {
            return
                new PrintIfUnread(
                    DateTime.Now.AddDays(3),
                    GetPrintDetails()
                    );
        }

        public static V8.PrintIfUnread GetPrintIfUnreadTransferObject()
        {
            return new V8.PrintIfUnread
            {
                PrintIfUnreadAfter = DateTime.Now.AddDays(3),
                PrintDetails = GetPrintDetailsDataTransferObject()
            };
        }

        public static NorwegianAddress GetNorwegianAddress()
        {
            return new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");
        }

        public static ForeignAddress GetForeignAddress()
        {
            return new ForeignAddress(
                CountryIdentifier.Country,
                "NO",
                "Adresselinje1",
                "Adresselinje2",
                "Adresselinje3",
                "Adresselinje4");
        }

        public static IPrintRecipient GetPrintRecipientWithNorwegianAddress()
        {
            return new PrintRecipient("name", GetNorwegianAddress());
        }

        public static IPrintReturnRecipient GetPrintReturnRecipientWithNorwegianAddress()
        {
            return new PrintReturnRecipient("name", GetNorwegianAddress());
        }
    }
}
