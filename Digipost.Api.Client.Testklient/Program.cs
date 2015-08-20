using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApiClientShared;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.ConcurrencyTest;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identification;
using Digipost.Api.Client.Domain.PersonDetails;
using Digipost.Api.Client.Domain.Print;
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
            CompareLogic comparelogic = new CompareLogic();

            var v = comparelogic.Compare(new PersonDetails(){DigipostAddress = "Halloen"}, new PersonDetails());
            List<Difference> diff = v.Differences;
            var descr = diff.ElementAt(0).GetWhatIsCompared();

            //Performance();
            Send();
            Console.ReadKey();

            

        }

        private static void Performance()
        {
            Initializer.Run(); //concurency runner
        }

        private static void Send()
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
            SendMessageToPerson(api, true);
            var response = Search(api);

            
            //var res = api.GetPersonDetails(response.AutcompleteSuggestions[0]);
            //ConcurrencyTest.Initializer.Run(); //concurency runner
            
            Console.ReadKey();
        }

        private static PersonDetailsResult Search(DigipostClient api)
        {
            return api.Search("Kristian");
        }

        private static void SendMessageToPerson(DigipostClient api, bool isQaOrLocal = false)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Sending message:");
            Console.WriteLine("======================================");
            Message message;

            message = isQaOrLocal ? GetMessageForQaOrLocal() : GetMessage();
            
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
                WriteToConsoleWithColor("> Oh snap... " + e.Message + e.InnerException.Message, true);
            }
        }

        private static void IdentifyPerson(DigipostClient api)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Identifiserer person:");
            Console.WriteLine("======================================");

            var identification = new Identification(IdentificationChoice.OrganisationNumber, "994921608");
            //var identification = new Identification(IdentificationChoice.DigipostAddress, "jarand.bjarte.t.k.grindheim#8DVE");
            //var identification = new Identification(new RecipientByNameAndAddress("Jarand-Bjarte Tysseng Kvistdahl Grindheim", "0467", "Oslo", "Digipost Testgate 2A"));

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

        private static Message GetMessageForQaOrLocal()
        {
            //primary document
            var primaryDocument = new Document(subject: "Primary document", fileType: "txt", contentBytes: GetPrimaryDocument());
   
            var digitalRecipient = new Recipient(IdentificationChoice.PersonalidentificationNumber, "01013300001");

            var message = new Message(digitalRecipient, primaryDocument);
            
            return message;
        }

        private static Message GetMessage()
        {
            //primary document
            var primaryDocument = new Document(subject: "Primary document", fileType: "txt", contentBytes: GetPrimaryDocument());
            var invoice = new Invoice(subject: "Invoice 1", fileType: "txt", contentBytes: GetPrimaryDocument(), amount: 1, account: "18941362738", duedate: DateTime.Now, kid: "123123123");

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
            //var message = new Message(digitalRecipientWithFallbackPrint, invoice);
            var message = new Message(
                recipient: digitalRecipientWithFallbackPrint,
                primaryDocument: invoice) 
                {};
            
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

        //////////////////////////////////////////////////////////////////////
        /// TESTEXAMPLES    
        /// /////////////////////////////////////////////////////////////////


        private void Method1()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            var message = new Message(
                new Recipient(IdentificationChoice.PersonalidentificationNumber, "311084xxxx"),
                new Document(subject: "Attachment", fileType:"txt", path: @"c:\...\document.txt")
              );

            var result = client.SendMessage(message);
        }

        private void Method2()
        {
            //Init Client
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            //Compose Recipient by name and address
            var recipientByNameAndAddress = new RecipientByNameAndAddress(
                fullName: "Ola Nordmann",
                addressLine: "Prinsensveien 123",
                postalCode: "0460",
                city: "Oslo"
               );

            //Compose message
            var message = new Message(
                new Recipient(recipientByNameAndAddress),
                new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf")
                );

            var result = client.SendMessage(message);
        }

        private void Method3()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");
            var attachment1 = new Document(subject: "Attachment 1", fileType: "txt", path: @"c:\...\attachment_01.txt");
            var attachment2 = new Document(subject: "Attachment 2", fileType: "pdf", path: @"c:\...\attachment_02.pdf");

            var message = new Message(
                new Recipient(IdentificationChoice.PersonalidentificationNumber, id: "241084xxxxx"), primaryDocument
                ) { Attachments = { attachment1, attachment2 } };

            var result = client.SendMessage(message);

            Logging.Log(TraceEventType.Information, result.Status.ToString());
        }

        private void Method4()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");
            var attachment1 = new Document(subject: "Attachment 1", fileType: "txt", path: @"c:\...\attachment_01.txt");
            var attachment2 = new Document(subject: "Attachment 2", fileType: "pdf", path: @"c:\...\attachment_02.pdf");

            var message = new Message(
                new Recipient(IdentificationChoice.PersonalidentificationNumber, id: "241084xxxxx"), primaryDocument
                ) { Attachments = { attachment1, attachment2 } };

            var result = client.SendMessage(message);

            Logging.Log(TraceEventType.Information, result.Status.ToString());
        }

        private void Method5()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            //recipientIdentifier for digital mail
            var recipientByNameAndAddress = new RecipientByNameAndAddress(
                fullName: "Ola Nordmann",
                postalCode: "0460",
                city: "Oslo",
                addressLine: "Prinsensveien 123");

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(
                    recipient: new PrintRecipient(
                        "Ola Nordmann",
                        new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                    printReturnAddress: new PrintReturnAddress(
                        "Kari Nordmann",
                        new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
                    );

            //recipient
            var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress, printDetails);

            var message = new Message(
                new Recipient(recipientByNameAndAddress, printDetails),
                new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf")
               );

            var result = client.SendMessage(message);
        }

        private void Method6()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            //recipientIdentifier for digital mail
            var recipientByNameAndAddress = new RecipientByNameAndAddress(
                fullName: "Ola Nordmann",
                postalCode: "0460",
                city: "Oslo",
                addressLine: "Prinsensveien 123");

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(
                    recipient: new PrintRecipient(
                        "Ola Nordmann",
                        new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                    printReturnAddress: new PrintReturnAddress(
                        "Kari Nordmann",
                        new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
                    );

            //recipient
            var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress, printDetails);

            var message = new Message(
                new Recipient(recipientByNameAndAddress, printDetails),
                new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf")
               );

            var result = client.SendMessage(message);
        }

        private void Method7()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            //recipientIdentifier for digital mail
            var recipientByNameAndAddress = new RecipientByNameAndAddress(
                fullName: "Ola Nordmann",
                postalCode: "0460",
                city: "Oslo",
                addressLine: "Prinsensveien 123");

            //printdetails for fallback to print (physical mail)
            var printDetails =
                new PrintDetails(
                    recipient: new PrintRecipient(
                        "Ola Nordmann",
                        new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                    printReturnAddress: new PrintReturnAddress(
                        "Kari Nordmann",
                        new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
                    );

            //recipient
            var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress, printDetails);

            var message = new Message(
                new Recipient(recipientByNameAndAddress, printDetails),
                new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf")
               );

            var result = client.SendMessage(message);
        }

        private void Method8()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            // API URL is different when request is sent from NHN
            config.ApiUrl = new Uri("https://api.nhn.digipost.no");

            var message = new Message(
                new Recipient(IdentificationChoice.PersonalidentificationNumber, "311084xxxx"),
                new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
              );

            var result = client.SendMessage(message);
        }

        private void Method9()
        {
            var config = new ClientConfig(senderId: "xxxxx");
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");
            
            var message = new Message(
                new Recipient(IdentificationChoice.PersonalidentificationNumber, "211084xxxx"),
                new Invoice(subject: "Invoice 1", fileType: "pdf", path: @"c:\...\invoice.pdf", amount: new decimal(100.21), account: "2593143xxxx", duedate: DateTime.Parse("01.01.2016"), kid: "123123123")
                );
            
            var result = client.SendMessage(message);
        }

        private void Method10()
        {

        }

        private void Method11()
        {

        }
    }
}