using System;
using System.Diagnostics;
using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
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
            var config = new ClientConfig(SenderId);
            config.ApiUrl = new Uri("https://api.digipost.no");

            Logging.Initialize(config);

            var api = new DigipostClient(config, Thumbprint);

            var message = GetMessage();
            try
            {
                var digipostClientResponse = api.SendMessage(message);
                Logging.Log(TraceEventType.Information, "\n" + digipostClientResponse);
            }
            catch (ClientResponseException e)
            {
                Logging.Log(TraceEventType.Information, "\n" + e.Error);
            }
            catch (Exception e)
            {
                Logging.Log(TraceEventType.Error, "\n" + "Nåkka gikk fette galt.");
            }


            var identification = new Identification(IdentificationChoice.PersonalidentificationNumber,"31108446911");
            
            try
            {
                var identificationResponse = api.Identify(identification);
                Logging.Log(TraceEventType.Information, "\n" + identificationResponse);
            }
            catch (ClientResponseException e)
            {
                var errorMessage = e.Error;
                Logging.Log(TraceEventType.Information, "\n" + errorMessage);
            }
            catch (Exception e)
            {
                Logging.Log(TraceEventType.Error, "\n" + "Nåkka gikk fette galt.");
            }


            var identificationByNameAndAddress = new Identification(IdentificationChoice.NameAndAddress, new RecipientByNameAndAddress("Kristian Sæther Enge","Collettsgate 68","0460","Oslo"));

            try
            {
                var identificationResponse = api.Identify(identificationByNameAndAddress);
                Logging.Log(TraceEventType.Information, "\n" + identificationResponse);
            }
            catch (ClientResponseException e)
            {
                var errorMessage = e.Error;
                Logging.Log(TraceEventType.Information, "\n" + errorMessage);
            }
            catch (Exception e)
            {
                Logging.Log(TraceEventType.Error, "\n" + "Nåkka gikk fette galt.");
            }

            Console.ReadKey();
        }

        private static Message GetMessage()
        {
            //primary document
            var primaryDocument = new Document("Primary document", "txt", GetPrimaryDocument());

            //attachment
            var attachment = new Document("Attachment", "txt", GetAttachment());

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(
                    new PrintRecipient("Kristian Sæther Enge", new NorwegianAddress("0460", "Oslo", "Colletts gate 68")),
                    new PrintReturnAddress("Kristian Sæther Enge",
                        new NorwegianAddress("0460", "Oslo", "Colletts gate 68"))
                    );


            //recipientIdentifier for digital mail
            var recipientByNameAndAddress = new RecipientByNameAndAddress("Kristian Sæther Enge", "Collettsgate 68",
                "0460", "Oslo");

            //recipient
            var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress,printDetails);

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