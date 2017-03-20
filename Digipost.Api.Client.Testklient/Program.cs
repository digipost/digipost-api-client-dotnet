using System;
using System.Reflection;
using Common.Logging;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.ConcurrencyTest;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Resources.Content;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string Thumbprint = "d8 6e 19 1b 8f 9b 0b 57 3e db 72 db a8 09 1f dc 6a 10 18 fd";
        private static readonly string SenderId = "779051";

        private static void Main(string[] args)
        {
            Log.Debug("Starting console application ...");
            RunSingle();
            Console.ReadKey();
        }

        private static void RunSingle()
        {
            var config = new ClientConfig(SenderId, Environment.Production);

            //Logging.Initialize(config);
            var api = new DigipostClient(config, Thumbprint);

            IdentifyPerson(api);
            SendMessageToPerson(api);
            //var response = Search(api);

            //var res = api.GetPersonDetails(response.AutcompleteSuggestions[0]);
            //ConcurrencyTest.Initializer.Run(); //concurency runner

            Console.ReadKey();
        }

        private static void Performance()
        {
            Initializer.Run(); //concurency runner
        }

        private static ISearchDetailsResult Search(DigipostClient api)
        {
            return api.Search("Al");
        }

        private static async void SendMessageToPerson(DigipostClient api, bool isQaOrLocal = false)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Sending message:");
            Console.WriteLine("======================================");
            IMessage message;

            message = isQaOrLocal ? GetMessageWithRecipientByIdForQaOrLocal() : GetMessage();

            try
            {
                var messageDeliveryResult = await api.SendMessageAsync(message).ConfigureAwait(false);
                Console.WriteLine("> Starter å sende melding");
                WriteToConsoleWithColor("Meldingens status: " + messageDeliveryResult.Status);
                WriteToConsoleWithColor("> Alt gikk fint!");
            }
            catch (AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is ClientResponseException)
                    {
                        Console.WriteLine("This really failed!");
                        return true;
                    }
                    return false;
                });
            }
            catch (ClientResponseException e)
            {
                var errorMessage = e.Error;
                WriteToConsoleWithColor("> Error." + errorMessage, true);
            }
            catch (Exception e)
            {
                WriteToConsoleWithColor("> Oh snap... " + e.Message + e.InnerException?.Message, true);
            }
        }

        private static void IdentifyPerson(DigipostClient api)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Identifiserer person:");
            Console.WriteLine("======================================");
            var identification = new Identification(new RecipientById(IdentificationType.OrganizationNumber, "896295291"));

            try
            {
                var identificationResponse = api.Identify(identification);
                Log.Debug("Identification resp: \n" + identificationResponse);
                WriteToConsoleWithColor("> Personen ble identifisert!");

                Console.WriteLine("ResultType: " + identificationResponse.ResultType);
                Console.WriteLine("IdentificationValue: " + identificationResponse.Data);
                Console.WriteLine("Error: " + identificationResponse.Error);
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

        private static IMessage GetMessageWithRecipientByIdForQaOrLocal()
        {
            //primary document
            var primaryDocument = new Document("Primary document", "txt", GetPrimaryDocument());

            var digitalRecipient = new RecipientById(IdentificationType.PersonalIdentificationNumber, "01013300001");

            var message = new Message("1010", digitalRecipient, primaryDocument);

            return message;
        }

        private static IMessage GetMessage()
        {
            //primary document
            var primaryDocument = new Document("Primary document", "txt", GetPrimaryDocument());
            var invoice = new Invoice("Invoice 1", "txt", GetPrimaryDocument(), 1, "18941362738", DateTime.Now, "123123123");

            //attachment
            var attachment = new Document("Attachment", "txt", GetAttachment());

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(
                    new PrintRecipient("Kristian Sæther Enge", new NorwegianAddress("0460", "Oslo", "Colletts gate 68")),
                    new PrintReturnRecipient("Kristian Sæther Enge",
                        new NorwegianAddress("0460", "Oslo", "Colletts gate 68"))
                );

            //message
            var message = new Message("1010", new RecipientById(IdentificationType.PersonalIdentificationNumber, "07068932715"), invoice);

            message.Attachments.Add(attachment);

            return message;
        }

        private static byte[] GetPrimaryDocument()
        {
            return ContentResource.Hoveddokument.PlainText();
        }

        private static byte[] GetAttachment()
        {
            return ContentResource.Vedlegg.Text();
        }

        private static void WriteToConsoleWithColor(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}