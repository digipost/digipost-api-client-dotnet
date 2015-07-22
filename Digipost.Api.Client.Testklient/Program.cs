using System;
using System.Diagnostics;
using ApiClientShared;
using Digipost.Api.Client.Api;
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
            var config = new ClientConfig(SenderId)
            {
                ApiUrl = new Uri("https://api.digipost.no"),
                Logger = (severity, konversasjonsId, metode, melding) =>
                {
                    Debug.WriteLine("{0} - {1} [{2}]",
                        DateTime.Now,
                        melding,
                        konversasjonsId.GetValueOrDefault()
                    );
                }
            };

            //Logging.Initialize(config);
            var api = new DigipostClient(config, Thumbprint);

            IdentifyPerson(api);
            SendMessageToPerson(api);
            //ConcurrencyTest.Initializer.Run(); //concurency runner
            
            
            Console.ReadKey();
        }

        private static void SendMessageToPerson(DigipostClient api)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Sending message:");
            Console.WriteLine("======================================");
            var message = GetMessage();
            try
            {
                Console.WriteLine("> Starter å sende melding");
                var messageDeliveryResult = api.SendMessage(message);
                Logging.Log(TraceEventType.Information, ""+messageDeliveryResult);
                WriteToConsoleWithColor("> Alt gikk fint!" , false);
            }
            catch (ClientResponseException e)
            {
                var errorMessage = e.Error;
                WriteToConsoleWithColor("> Error." + errorMessage, true);
            }
            catch (Exception e)
            {
                WriteToConsoleWithColor("> Oh snap... " + e.Message, true);
            }
        }

        private static void IdentifyPerson(DigipostClient api)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Identifiserer person:");
            Console.WriteLine("======================================");

            var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "31108446911");

            try
            {
                var identificationResponse = api.Identify(identification);
                Logging.Log(TraceEventType.Information, "Identification resp: \n" + identificationResponse);
                WriteToConsoleWithColor("> Personen ble identifisert!", false);
            }
            catch (ClientResponseException e)
            {
                var errorMessage = e.Error;
                WriteToConsoleWithColor("> Feilet." + errorMessage, true);
            }
            catch (Exception e)
            {
                WriteToConsoleWithColor("> Oh snap... " + e.Message, true);
            }
        }

        private static Message GetMessage()
        {
            //primary document
            var primaryDocument = new Document(subject: "Primary document", fileType: "txt", contentBytes: GetPrimaryDocument());
            var invoice = new Invoice(subject: "Invoice 1", fileType: "txt", contentBytes: GetPrimaryDocument(), amount: 1, account: "15941432384", duedate: DateTime.Now, kid: "123123123");

            //attachment
            var attachment = new Document("Attachment", "txt", GetAttachment());

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(
                    new PrintRecipient("Kristian Sæther Enge", new NorwegianAddress(postalCode: "0460", city: "Oslo", addressline1: "Colletts gate 68")),
                    new PrintReturnAddress("Kristian Sæther Enge",
                        new NorwegianAddress("0460", "Oslo", "Colletts gate 68"))
                    );



            //recipientIdentifier for digital mail
            var recipientByNameAndAddress = new RecipientByNameAndAddress("Kristian Sæther Enge", "0460",
                "Oslo", "Collettsgate 68");

            //recipient
            var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress);

            //message
            var message = new Message(digitalRecipientWithFallbackPrint, invoice);
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

        private static void WriteToConsoleWithColor(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}