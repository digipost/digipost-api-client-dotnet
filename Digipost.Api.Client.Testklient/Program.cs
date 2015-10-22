using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApiClientShared;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.ConcurrencyTest;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Testklient.Properties;
using KellermanSoftware.CompareNetObjects;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private static readonly string Thumbprint = Settings.Default.ThumbprintDnBLocalQa;
        private static readonly string SenderId = Settings.Default.SenderIdDnbQa2;
        private static readonly string Url = Settings.Default.Url;

        private static readonly ResourceUtility ResourceUtility =
            new ResourceUtility("Digipost.Api.Client.Testklient.Resources");

        private static void Main(string[] args)
        {
            //Performance();
            RunSingle();
            
            Console.ReadKey();
        }

        private static void Performance()
        {
            Initializer.Run(); //concurency runner
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
                var messageDeliveryResult = await api.SendMessageAsync(message);
                Console.WriteLine("> Starter å sende melding");
                WriteToConsoleWithColor("Meldingens status: " + messageDeliveryResult.Status);
                WriteToConsoleWithColor("> Alt gikk fint!", false);
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
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

                WriteToConsoleWithColor("> Oh snap... " + e.Message + e.InnerException.Message, true);
            }
        }

        private static void IdentifyPerson(DigipostClient api)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Identifiserer person:");
            Console.WriteLine("======================================");

            //var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "01013300001");
            //var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "31010986802"));
            //var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "16014139692"));
            //var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "01108448586"));
            //var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "01108448511"));
            // var identification = new Identification(new RecipientByNameAndAddress("Kristian Sæther Enge","Collettsgate 68","0460","Oslo"));
            //var identification = new Identification(new RecipientByNameAndAddress("Kristian Sæther Enge", "blåbærveien 1", "9999", "Oslo"));
            var identification = new Identification(new RecipientById(IdentificationType.OrganizationNumber, "896295291"));

            try
            {
                var identificationResponse = api.Identify(identification);
                Logging.Log(TraceEventType.Information, "Identification resp: \n" + identificationResponse);
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
            var primaryDocument = new Document(subject: "Primary document", fileType: "txt", contentBytes: GetPrimaryDocument());
   
            var digitalRecipient = new RecipientById(IdentificationType.PersonalIdentificationNumber, "01013300001");

            var message = new Message(digitalRecipient, primaryDocument);

            return message;
        }

        private static IMessage GetMessage()
        {
            //primary document
            var primaryDocument = new Document(subject: "Primary document", fileType: "txt", contentBytes: GetPrimaryDocument());
            var invoice = new Invoice(subject: "Invoice 1", fileType: "txt", contentBytes: GetPrimaryDocument(), amount: 1, account: "18941362738", duedate: DateTime.Now, kid: "123123123");

            //attachment
            var attachment = new Document("Attachment", "txt", GetAttachment());

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(
                    new PrintRecipient("Kristian Sæther Enge", new NorwegianAddress("0460", "Oslo","Colletts gate 68")),
                    new PrintReturnRecipient("Kristian Sæther Enge",
                        new NorwegianAddress("0460", "Oslo", "Colletts gate 68"))
                    );

            //recipientIdentifier for digital mail
            var recipientByNameAndAddress = new RecipientByNameAndAddressDataTranferObject("Kristian Sæther Enge", "0460",
                "Oslo", "Collettsgate 68");

            //Nytt regime for message
            var recipientByNameAndAddressNew = new RecipientByNameAndAddress("Kristian Sæther Enge", "0460",
                "Oslo", "Collettsgate 68");


            var recipientById = new RecipientById(IdentificationType.DigipostAddress, "jarand.bjarte.t.k.grindheim#71WZ");

            //End nytt regime for message
            
            //message
            //var message = new Message(digitalRecipientWithFallbackPrint, invoice);
            var message = new Message(recipientById, invoice);
            
            //message.Deliverytime = DateTime.Now.AddDays(1);

            message.Attachments.Add(attachment);
            //message.SenderOrganization = new SenderOrganization(){OrganizationNumber = "123Sammahvilkenid", unitId = "Partid"};
            //message.SenderId = 12333;
            
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