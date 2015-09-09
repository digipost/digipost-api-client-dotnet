﻿using System;
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
            
            uc02();
            //Performance();
            //Send();
            Console.ReadKey();

            

        }

        private static readonly string yourThumbprint = "d8 6e 19 1b 8f 9b 0b 57 3e db 72 db a8 09 1f dc 6a 10 18 fd";
        private static readonly string yourSenderId = "106818904";
        private static readonly string yourPersonalIdentification = "31108446911";
        private static readonly string filePathString = "E:\\TestDocument.txt";
        private static readonly string URIToDigipost = "https://qa2.api.digipost.no";
        public static void uc02()
        {

            //Init Client
            var config = new ClientConfig(senderId: yourSenderId) { ApiUrl = new Uri(URIToDigipost) };
            var client = new DigipostClient(config, thumbprint: yourThumbprint);

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
                new Document(subject: "document subject", fileType: "txt", path: filePathString)
                );

            try
            {
                var result = client.SendMessage(message);
            }
            catch (Exception e)
            {
                
                
            }
            
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

            //IdentifyPerson(api);
            SendMessageToPerson(api, false);
            var response = Search(api);

            
            //var res = api.GetPersonDetails(response.AutcompleteSuggestions[0]);
            //ConcurrencyTest.Initializer.Run(); //concurency runner
            
            Console.ReadKey();
        }

        private static ISearchDetailsResult Search(DigipostClient api)
        {
            return api.Search("Al");
        }

        private static void SendMessageToPerson(DigipostClient api, bool isQaOrLocal = false)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Sending message:");
            Console.WriteLine("======================================");
            IMessage message;

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

            //var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "01013300001");
            var identification = new Identification(IdentificationChoiceType.DigipostAddress, "jarand.bjarte.t.k.grindheim#8DVE");


            try
            {
                var identificationResponse = api.Identify(identification);
                Logging.Log(TraceEventType.Information, "Identification resp: \n" + identificationResponse);
                WriteToConsoleWithColor("> Personen ble identifisert!", false);
                
                //Console.WriteLine("IdentificationValue: " + identificationResponse.IdentificationValue);
                //Console.WriteLine("IdentificationType: " + identificationResponse.IdentificationType)
                ;

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

        private static IMessage GetMessageForQaOrLocal()
        {
            //primary document
            var primaryDocument = new Document(subject: "Primary document", fileType: "txt", contentBytes: GetPrimaryDocument());
   
            var digitalRecipient = new Recipient(IdentificationChoiceType.PersonalidentificationNumber, "01013300001");

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
            var recipientByNameAndAddress = new RecipientByNameAndAddress("Kristian Sæther Enge", "0460",
                "Oslo", "Collettsgate 68");

            //recipient
            var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress, printDetails);

            //message
            //var message = new Message(digitalRecipientWithFallbackPrint, invoice);
            var message = new Message(digitalRecipientWithFallbackPrint,invoice) 
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
    }
}