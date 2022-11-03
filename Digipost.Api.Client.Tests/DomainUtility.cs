using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Resources.Content;
using Digipost.Api.Client.Send;
using V8;
using Document = Digipost.Api.Client.Send.Document;
using Environment = Digipost.Api.Client.Common.Environment;
using Identification = Digipost.Api.Client.Common.Identify.Identification;
using Message = V8.Message;

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

        public static Message GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById()
        {
            var message = new Message()
            {
                Sender_Id = long.Parse("1010"),
                Message_Id = "ThatMessageId",
                Primary_Document = new V8.Document()
                {
                    Subject = "TestSubject",
                    File_Type = "txt",
                    Uuid = "attachmentGuidPrimary",
                    Authentication_LevelSpecified = true,
                    Sensitivity_LevelSpecified = true
                },
                Delivery_Time = DateTime.Today.AddDays(3),
                Delivery_TimeSpecified = true,
                Recipient = new Message_Recipient()
                {
                    Digipost_Address = "ola.nordmann#246BB"
                }
            };
            message.Attachment.Add(new V8.Document()
            {
                Subject = "TestSubject attachment",
                File_Type = "txt",
                Uuid = "attachmentGuid",
                Authentication_LevelSpecified = true,
                Sensitivity_LevelSpecified = true
            });

            return message;
        }

        public static IMessage GetSimpleMessageWithRecipientByNameAndAddress()
        {
            return new Send.Message(GetSender(), GetRecipientByNameAndAddress(), GetDocument());
        }

        public static IDocument GetDocument(string dataType = null)
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

        public static Print_Details GetPrintDetailsDataTransferObject()
        {
            return new Print_Details
            {
                Recipient = new Print_Recipient()
                {
                    Name = "Ola Nordmann",
                    Norwegian_Address = new Norwegian_Address
                    {
                        Addressline1 = "Osloveien 15",
                        City = "Oslo",
                        Zip_Code = "0115"
                    }
                },
                Return_Address = new Print_Recipient
                {
                    Name = "Returkongen",
                    Norwegian_Address = new Norwegian_Address()
                    {
                        Addressline1 = "Sophauggata 22",
                        City = "Sophaugen",
                        Zip_Code = "5510"
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

        public static Print_If_Unread GetPrintIfUnreadTransferObject()
        {
            return new Print_If_Unread
            {
                Print_If_Unread_After = DateTime.Now.AddDays(3),
                Print_Details = GetPrintDetailsDataTransferObject()
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
