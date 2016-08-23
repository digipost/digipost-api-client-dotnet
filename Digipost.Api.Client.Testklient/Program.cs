using System;
using System.Reflection;
using ApiClientShared;
using Common.Logging;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.ConcurrencyTest;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string Thumbprint = "19 f6 af 36 98 b1 3a c5 67 93 34 fb c3 f5 5b b0 8d 89 e5 2f";
        private static readonly string SenderId = "779052";
        private static readonly string Url = "https://qa.api.digipost.no/";

        private static readonly ResourceUtility ResourceUtility =
            new ResourceUtility("Digipost.Api.Client.Testklient.Resources");

        private static void Main(string[] args)
        {
            Log.Debug("Starting console application ...");
            RunSingle();
            Console.ReadKey();
        }

        private static void RunSingle()
        {
            var config = new ClientConfig(SenderId)
            {
                ApiUrl = new Uri(Url),
                Logger = (severity, konversasjonsId, metode, melding) =>
                {
                    Console.WriteLine("{0}",
                        melding
                        );
                }
            };

            //Logging.Initialize(config);
            var api = new DigipostClient(config, Thumbprint);

            IdentifyPerson(api);
            //SendMessageToPerson(api, false);
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
                WriteToConsoleWithColor("> Alt gikk fint!", false);
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
                WriteToConsoleWithColor("> Personen ble identifisert!", false);

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

            var message = new Message(digitalRecipient, primaryDocument);

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
            var message = new Message(new RecipientById(IdentificationType.PersonalIdentificationNumber, "XX"), invoice);

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