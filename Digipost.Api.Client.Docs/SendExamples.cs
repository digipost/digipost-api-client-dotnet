﻿using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.DataTypes.Core;
using Digipost.Api.Client.Send;
using Address = Digipost.Api.Client.DataTypes.Core.Common.Address;
using Environment = Digipost.Api.Client.Common.Environment;

#pragma warning disable 0649

namespace Digipost.Api.Client.Docs
{
    public class SendExamples
    {
        private static readonly DigipostClient client;
        private static readonly Sender sender = new Sender(67890);

        public void ClientConfigurationWithThumbprint()
        {
            // The actual sender of the message. The broker is the owner of the organization certificate
            // used in the library. The broker id can be retrieved from your Digipost organization account.
            var broker = new Broker(12345);

            var clientConfig = new ClientConfig(broker, Environment.Production);
            var client = new DigipostClient(clientConfig, thumbprint: "84e492a972b7e...");
        }

        public void ClientConfigurationWithCertificate()
        {
            // The actual sender of the message. The broker is the owner of the organization certificate
            // used in the library. The broker id can be retrieved from your Digipost organization account.
            var broker = new Broker(12345);

            // The sender is what the receiver of the message sees as the sender of the message.
            // Sender and broker id will both be your organization's id if you are sending on behalf of yourself.
            var sender = new Sender(67890);

            var clientConfig = new ClientConfig(broker, Environment.Production);

            var client = new DigipostClient(clientConfig, CertificateReader.ReadCertificate());
        }

        public void SendOneLetterToRecipientViaPersonalIdentificationNumber()
        {
            var message = new Message(
                sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
                new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
            );

            var result = client.SendMessage(message);
        }

        public void SendOneLetterViaNameAndAddress()
        {
            var recipient = new RecipientByNameAndAddress(
                fullName: "Ola Nordmann",
                addressLine1: "Prinsensveien 123",
                postalCode: "0460",
                city: "Oslo"
            );

            var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

            var message = new Message(sender, recipient, primaryDocument);
            var result = client.SendMessage(message);
        }

        public void WithMultipleAttachments()
        {
            var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");
            var attachment1 = new Document(subject: "Attachment 1", fileType: "txt", path: @"c:\...\attachment_01.txt");
            var attachment2 = new Document(subject: "Attachment 2", fileType: "pdf", path: @"c:\...\attachment_02.pdf");

            var message = new Message(
                sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, id: "241084xxxxx"),
                primaryDocument
            ) {Attachments = {attachment1, attachment2}};

            var result = client.SendMessage(message);
        }

        public void SendLetterWithSmsNotification()
        {
            var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");

            primaryDocument.SmsNotification = new SmsNotification(afterHours: 0); //SMS reminder after 0 hours
            primaryDocument.SmsNotification.NotifyAtTimes.Add(new DateTime(2015, 05, 05, 12, 00, 00)); //new reminder at a specific date

            var message = new Message(
                sender,
                new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx"),
                primaryDocument
            );

            var result = client.SendMessage(message);
        }

        public void SendLetterWithFallbackToPrint()
        {
            var recipient = new RecipientByNameAndAddress(
                fullName: "Ola Nordmann",
                addressLine1: "Prinsensveien 123",
                postalCode: "0460",
                city: "Oslo"
            );

            var printDetails =
                new PrintDetails(
                    printRecipient: new PrintRecipient(
                        "Ola Nordmann",
                        new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                    printReturnRecipient: new PrintReturnRecipient(
                        "Kari Nordmann",
                        new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
                );

            var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

            var messageWithFallbackToPrint = new Message(sender, recipient, primaryDocument)
            {
                PrintDetails = printDetails
            };

            var result = client.SendMessage(messageWithFallbackToPrint);
        }

        public void SendLetterWithDirectToPrint()
        {
            var printDetails =
                new PrintDetails(
                    printRecipient: new PrintRecipient(
                        "Ola Nordmann",
                        new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                    printReturnRecipient: new PrintReturnRecipient(
                        "Kari Nordmann",
                        new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
                );

            var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

            var messageToPrint = new PrintMessage(sender, printDetails, primaryDocument);

            var result = client.SendMessage(messageToPrint);
        }

        public void SendLetterWithPrintIfUnread()
        {
            var recipient = new RecipientByNameAndAddress(
                fullName: "Ola Nordmann",
                addressLine1: "Prinsensveien 123",
                postalCode: "0460",
                city: "Oslo"
            );

            var printDetails =
                new PrintDetails(
                    printRecipient: new PrintRecipient(
                        "Ola Nordmann",
                        new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                    printReturnRecipient: new PrintReturnRecipient(
                        "Kari Nordmann",
                        new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
                );

            var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

            var messageWithPrintIfUnread = new Message(sender, recipient, primaryDocument)
            {
                PrintDetails = printDetails,
                DeliveryTime = DateTime.Now.AddDays(3),
                PrintIfUnread = new PrintIfUnread(DateTime.Now.AddDays(6), printDetails)
            };

            var result = client.SendMessage(messageWithPrintIfUnread);
        }

        public void SendLetterWithRequestForRegistration()
        {
            var recipient = new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx");

            var requestForRegistration = new RequestForRegistration(
                DateTime.Now.AddDays(3),
                "+4711223344",
                null,
                new PrintDetails(
                    printRecipient: new PrintRecipient(
                        "Ola Nordmann",
                        new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                    printReturnRecipient: new PrintReturnRecipient(
                        "Kari Nordmann",
                        new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
                )
            );

            var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

            var messageWithPrintIfUnread = new Message(sender, recipient, primaryDocument)
            {
                Id = "Reg-12345",
                RequestForRegistration = requestForRegistration
            };

            var result = client.SendMessage(messageWithPrintIfUnread);
        }

        public void SendLetterWithHigherSecurityLevel()
        {
            var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf")
            {
                AuthenticationLevel = AuthenticationLevel.TwoFactor, // Require BankID or BuyPass to open letter
                SensitivityLevel = SensitivityLevel.Sensitive // Sender information and subject will be hidden until Digipost user is logged in at the appropriate authentication level
            };

            var message = new Message(
                sender,
                new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx"),
                primaryDocument
            );

            var result = client.SendMessage(message);
        }

        public void IdentifyRecipient()
        {
            var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxxx"));
            var identificationResponse = client.Identify(identification);

            if (identificationResponse.ResultType == IdentificationResultType.DigipostAddress)
            {
                //Exist as user in Digipost.
                //If you used personal identification number to identify - use this to send a message to this individual.
                //If not, see Data field for DigipostAddress.
            }
            else if (identificationResponse.ResultType == IdentificationResultType.Personalias)
            {
                //The person is identified but does not have an active Digipost account.
                //You can continue to use this alias to check the status of the user in future calls.
            }
            else if (identificationResponse.ResultType == IdentificationResultType.InvalidReason ||
                     identificationResponse.ResultType == IdentificationResultType.UnidentifiedReason)
            {
                //The person is NOT identified. Check Error for more details.
            }
        }

        public void SendLetterThroughNorskHelsenett()
        {
            // API URL is different when request is sent from NHN
            var config = new ClientConfig(new Broker(12345), Environment.NorskHelsenett);
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

            var message = new Message(
                sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
                new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
            );

            var result = client.SendMessage(message);
        }

        public void SendInvoice()
        {
            var invoice = new Invoice(dueDate: DateTime.Parse("2022-12-03T10:15:30+01:00 Europe/Paris"), sum: new decimal(100.21), creditorAccount: "2593143xxxx")
            {
                Kid = "123123123"
            };

            var message = new Message(
                sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxx"),
                new Document(
                    subject: "Invoice 1",
                    fileType: "pdf",
                    path: @"c:\...\invoice.pdf",
                    dataType: invoice)
            );

            var result = client.SendMessage(message);
        }

        public void SendInkasso()
        {
            var inkasso = new Inkasso(dueDate: DateTime.Parse("2022-12-03T10:15:30+01:00 Europe/Paris"))
            {
                Kid = "123123123",
                Sum = new decimal(100.21),
                Account = "2593143xxxx"
            };

            var message = new Message(
                sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxx"),
                new Document(
                    subject: "Invoice 1",
                    fileType: "pdf",
                    path: @"c:\...\invoice.pdf",
                    dataType: inkasso)
            );

            var result = client.SendMessage(message);
        }

        public void SearchForReceivers()
        {
            var response = client.Search("Ola Nordmann Bellsund Longyearbyen");

            foreach (var person in response.PersonDetails)
            {
                var digipostAddress = person.DigipostAddress;
                var phoneNumber = person.MobileNumber;
            }
        }

        public void SendOnBehalfOfOrganization()
        {
            var broker = new Broker(12345);
            var sender = new Sender(67890);

            var digitalRecipient = new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx");
            var primaryDocument = new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt");

            var clientConfig = new ClientConfig(broker, Environment.Production);

            var message = new Message(sender, digitalRecipient, primaryDocument);

            var result = client.SendMessage(message);
        }

        public void SendMessageWithDeliveryTime()
        {
            var message = new Message(
                sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
                new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
            )
            {
                DeliveryTime = DateTime.Now.AddDays(1).AddHours(4)
            };

            var result = client.SendMessage(message);
        }

        public void SendMessageWithAppointmentMetadata()
        {
            var appointment = new Appointment(startTime: DateTime.Parse("2017-11-24T13:00:00+0100"))
            {
                EndTime = DateTime.Parse("2017-11-24T13:00:00+0100").AddMinutes(30),
                Address = new Address
                {
                    StreetAddress = "Storgata 1",
                    PostalCode = "0001",
                    City = "Oslo"
                }
            };

            var document = new Document(
                subject: "Your appointment",
                fileType: "pdf",
                path: @"c:\...\document.pdf",
                dataType: appointment
            );
        }

        public void SendMessageWithExternalLinkMetadata()
        {
            var externalLink = new ExternalLink(absoluteUri: new Uri("https://example.org/loan-offer/uniqueCustomerId/"))
            {
                Description = "Please read the terms, and use the button above to accept them. The offer expires at 23/10-2018 10:00.",
                ButtonText = "Accept offer",
                Deadline = DateTime.Parse("2018-10-23T10:00:00+0200")
            };

            var document = new Document(
                subject: "Your appointment",
                fileType: "pdf",
                path: @"c:\...\document.pdf",
                dataType: externalLink
            );

            // Create Message and send using the client as specified in other examples.
        }

        public void SendMessageWithSenderInformation()
        {
            var senderInformation = client.GetSenderInformation(new SenderOrganisation("9876543210", "thePartId"));

            var message = new Message(
                senderInformation.Sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
                new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
            );

            var result = client.SendMessage(message);
        }
    }
}
