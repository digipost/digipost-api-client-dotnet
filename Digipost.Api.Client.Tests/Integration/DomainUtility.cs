using System;
using System.Collections.Generic;
using ApiClientShared;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Tests.Integration
{
    public class DomainUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");

        public static IMessage GetSimpleMessageWithRecipientById()
        {
            var message = new Message(
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"),
                GetDocument()
                );
            return message;
        }

        public static IMessage GetMessageWithBytesAndStaticGuidRecipientById()
        {
            var deliverytime = DateTime.Today.AddDays(3);
            var recipientById = GetRecipientByDigipostId();

            return new Message(
                    recipientById,
                    new Document("TestSubject", "txt", new byte[3])
                )
                {
                    SenderId = "SenderId",
                    Attachments = new List<IDocument>()
                    {
                        new Document("TestSubject attachment", "txt",  new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                    DeliveryTime = deliverytime,
                    PrimaryDocument = { Guid = "attachmentGuidPrimary" }
                };
        }

        public static MessageDataTransferObject GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById()
        {
            var deliverytime = DateTime.Today.AddDays(3);
            var recipientDataTransferObject = GetRecipientByDigipostIdDataTransferObject();

            return new MessageDataTransferObject(
                    recipientDataTransferObject,
                    new DocumentDataTransferObject("TestSubject", "txt", new byte[3]), "SenderId")
                {
                    Attachments = new List<DocumentDataTransferObject>
                        {
                            new DocumentDataTransferObject("TestSubject attachment", "txt", new byte[3])
                            {
                                Guid = "attachmentGuid"
                            }
                        },
                    DeliveryTime = deliverytime,
                    PrimaryDocumentDataTransferObject = { Guid = "attachmentGuidPrimary" }
                };
        }

        public static IMessage GetMessageWithBytesAndStaticGuidRecipientByNameAndAddress()
        {
            var deliverytime = DateTime.Today.AddDays(3);
            var recipientById = GetRecipientByNameAndAddress();

            return new Message(
                    recipientById,
                    new Document("TestSubject", "txt", new byte[3])
                )
            {
                SenderId = "SenderId",
                Attachments = new List<IDocument>()
                    {
                        new Document("TestSubject attachment", "txt",  new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                DeliveryTime = deliverytime,
                PrimaryDocument = { Guid = "attachmentGuidPrimary" }
            };
        }

        public static MessageDataTransferObject GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientNameAndAddress()
        {
            var deliverytime = DateTime.Today.AddDays(3);
            var recipientDataTransferObject = GetRecipientByNameAndAndAddressDataTransferObject();

            return new MessageDataTransferObject(
                    recipientDataTransferObject,
                    new DocumentDataTransferObject("TestSubject", "txt", new byte[3]), "SenderId"
                )
            {
                Attachments = new List<DocumentDataTransferObject>
                        {
                            new DocumentDataTransferObject("TestSubject attachment", "txt", new byte[3])
                            {
                                Guid = "attachmentGuid"
                            }
                        },
                DeliveryTime = deliverytime,
                PrimaryDocumentDataTransferObject = { Guid = "attachmentGuidPrimary" }
            };
        }

        public static IMessage GetSimpleMessageWithRecipientByNameAndAddress()
        {
            var message = new Message(
                GetRecipientByNameAndAddress(),
                GetDocument()
                );
            return message;
        }

        public static IDocument GetDocument()
        {
            return new Document("Integrasjonstjest", "txt", ResourceUtility.ReadAllBytes(true, "Hoveddokument.txt"));
        }

        public static IIdentification GetPersonalIdentification()
        {
            var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"));
            return identification;
        }

        public static IIdentification GetPersonalIdentificationById()
        {
            var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"));
            return identification;
        }

        public static IIdentification GetPersonalIdentificationByNameAndAddress()
        {
            var identification = new Identification(new RecipientByNameAndAddress("ola nordmann","0000","Oslo","Biskop gunnerius gate 14a"));
            return identification;
        }


        public static RecipientById GetRecipientByDigipostId()
        {
            return new RecipientById(IdentificationType.DigipostAddress, "ola.nordmann#246BB");
        }

        public static RecipientDataTransferObject GetRecipientByDigipostIdDataTransferObject()
        {
            return new RecipientDataTransferObject(IdentificationChoiceType.DigipostAddress, "ola.nordmann#246BB");
        }

        public static RecipientDataTransferObject GetRecipientByNameAndAndAddressDataTransferObject()
        {
            return new RecipientDataTransferObject(new RecipientByNameAndAddressDataTranferObject(
                fullName: "Ola Nordmann", 
                postalCode: "0001", 
                city: "Oslo",
                addressLine1: "Biskop Gunnerus Gate 14"
                )
             );
        }

        public static RecipientByNameAndAddress GetRecipientByNameAndAddress()
        {
            return new RecipientByNameAndAddress(
                fullName: "Ola Nordmann", 
                postalCode: "0001", 
                city: "Oslo", 
                addressLine1: "Biskop Gunnerus Gate 14"
                );
        }

        public static RecipientByNameAndAddressDataTranferObject GetRecipientByNameAndAddressDataTransferObject()
        {
            return new RecipientByNameAndAddressDataTranferObject(
                fullName: "Ola Nordmann",
                postalCode: "0001",
                city: "Oslo",
                addressLine1: "Osloveien 22"
                );
        }

        public static PrintDetails GetPrintDetails()
        {
            return 
                new PrintDetails(
                    new PrintRecipient("Ola Nordmann", new NorwegianAddress("0115", "Oslo" , "Osloveien 15" )),
                    new PrintReturnRecipient("Returkongen",
                        new NorwegianAddress("5510", "Sophaugen", "Sophauggata 22")));
        }

        public static PrintDetailsDataTransferObject GetPrintDetailsDataTransferObject()
        {
            return
                new PrintDetailsDataTransferObject(
                    new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0115", "Oslo", "Osloveien 15")), 
                    new PrintReturnRecipientDataTransferObject("Returkongen",
                        new NorwegianAddressDataTransferObject("5510", "Sophaugen", "Sophauggata 22")));
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