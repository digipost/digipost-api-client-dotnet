using System;
using System.Diagnostics;
using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private const string SenderId = "779052"; //"106768801";

        private static readonly ResourceUtility ResourceUtility =
            new ResourceUtility("Digipost.Api.Client.Testklient.Resources");

        private static readonly string Thumbprint = "84e492a972b7edc197a32d9e9c94ea27bd5ac4d9".ToUpper();

        private static void Main(string[] args)
        {
            var message = GetMessage();
            var config = new ClientConfig(SenderId);
            Logging.Initialize(config);

            var api = new DigipostClient(config, Thumbprint);

            var digipostClientResponse = api.Send(message);

            Logging.Log(TraceEventType.Information, "\n" + digipostClientResponse);

            Console.ReadKey();
        }

        private static Message GetMessage()
        {
            //primary document
            var primaryDocument = new Document("Primary document", "txt", GetPrimaryDocument())
            {
                SmsNotification = new SmsNotification(0) // SMS reminder after 0 hour
            };
            //attachment
            var attachment = new Document("Attachment", "txt", GetAttachment());

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(new PrintRecipient("Kristian Sæther Enge", "Colletts gate 68", "0460", "Oslo"),
                    new PrintRecipient("Kristian Sæther Enge", "Colletts gate 68", "0460", "Oslo")
                    );

            //recipientIdentifier for digital mail
            var recipientByNameAndAddress = new RecipientByNameAndAddress("Kristian Sæther Enge", "Collettsgate 68",
                "0460", "Oslo");

            //recipient
            var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress, printDetails);

            //message
            var message = new Message(digitalRecipientWithFallbackPrint, primaryDocument);
            message.Attachments.Add(attachment);

            return message;
        }

        private static byte[] GetPrimaryDocument()
        {
            return ResourceUtility.ReadAllBytes(true, "Hoveddokument.txt");
        }

        private static byte[] GetAttachment()
        {
            return ResourceUtility.ReadAllBytes(true, "Vedlegg.txt");
        }
    }
}