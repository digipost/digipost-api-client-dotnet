using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Resources.Content;
using Digipost.Api.Client.Send;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests
{
    public class DomainUtility
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
            var message = new Message(
                GetSender(),
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"),
                GetDocument()
            );
            return message;
        }

        public static IMessage GetMessageWithBytesAndStaticGuidRecipientById()
        {
            var deliverytime = DateTime.Today.AddDays(3);
            var recipientById = GetRecipientByDigipostId();

            return new Message(GetSender(), recipientById, new Document("TestSubject", "txt", new byte[3]))
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

        public static message GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById()
        {
            return new message
            {
                Item = long.Parse("1010"),
                messageid = "ThatMessageId",
                primarydocument = new document
                {
                    subject = "TestSubject",
                    filetype = "txt",
                    uuid = "attachmentGuidPrimary",
                    authenticationlevelSpecified = true,
                    sensitivitylevelSpecified = true
                },
                attachment = new[]
                {
                    new document
                    {
                        subject = "TestSubject attachment",
                        filetype = "txt",
                        uuid = "attachmentGuid",
                        authenticationlevelSpecified = true,
                        sensitivitylevelSpecified = true
                    }
                },
                deliverytime = DateTime.Today.AddDays(3),
                deliverytimeSpecified = true,
                recipient = new messagerecipient
                {
                    ItemElementName = ItemChoiceType1.digipostaddress,
                    Item = "ola.nordmann#246BB"
                }
            };
        }

        public static IMessage GetSimpleMessageWithRecipientByNameAndAddress()
        {
            return new Message(GetSender(), GetRecipientByNameAndAddress(), GetDocument());
        }

        public static IDocument GetDocument()
        {
            return new Document("simple-document-dotnet", "txt", ContentResource.Hoveddokument.PlainText());
        }

        public static IDocument GetInvoice()
        {
            return new Invoice("simple-invoice-dotnet", "pdf", ContentResource.Hoveddokument.Pdf(), 1005, "45278968788", DateTime.Now.AddDays(4));
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
                        new NorwegianAddress("5510", "Sophaugen", "Sophauggata 22")), PostType.A);
        }

        public static printdetails GetPrintDetailsDataTransferObject()
        {
            return new printdetails
            {
                recipient = new printrecipient
                {
                    name = "Ola Nordmann",
                    Item = new norwegianaddress
                    {
                        addressline1 = "Osloveien 15",
                        city = "Oslo",
                        zipcode = "0115"
                    }
                },
                returnaddress = new printrecipient
                {
                    name = "Returkongen",
                    Item = new norwegianaddress
                    {
                        addressline1 = "Sophauggata 22",
                        city = "Sophaugen",
                        zipcode = "5510"
                    }
                }
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
