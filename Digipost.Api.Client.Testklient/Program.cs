using System;
using System.Diagnostics;
using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
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
            DigipostClientResponse digipostClientResponse = null;
            try
            {
                digipostClientResponse = api.Send(message);
            }
            catch (Exception exception)
            {
                Logging.Log(TraceEventType.Error, exception.Message);
            }

            Logging.Log(TraceEventType.Information, digipostClientResponse.ToString());

            Console.ReadKey();
        }

        private static Message GetMessage()
        {
            //primary document
            var doc = new Document("Test", "txt", GetPrimaryDocument());

            //recipient
            var nameandaddr = new RecipientByNameAndAddress("Eirik Sæther Enge", "Enge gård", "2651", "Østre Gausdal");

            //printdetails
            var recieptAddress = new NorwegianAddress
            {
                Addressline1 = "Enge gård",
                City = "Østre Gausdal",
                ZipCode = "2651"
            };

            var returnAddress = new NorwegianAddress
            {
                Addressline1 = "Colletts gate 68",
                City = "Oslo",
                ZipCode = "0460"
            };

            var printRecipient = new PrintRecipient("Eirik Sæther Enge", recieptAddress);
            var printReturnAddress = new PrintRecipient("Kristian Sæther Enge", returnAddress);

            var printDetails = new PrintDetails(printRecipient, printReturnAddress)
            {
                NondeliverableHandling = NondeliverableHandling.Shred
            };

            var digitalMedFallbackPrint = new Recipient(nameandaddr, printDetails);

            var digital = new Recipient(nameandaddr);

            var fysiskPrint = new Recipient(new PrintDetails(printRecipient));

            //message
            var m = new Message(digital, doc);
            return m;
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