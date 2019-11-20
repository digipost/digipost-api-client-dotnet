using System;
using DataTypes;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Send;
using Address = DataTypes.Address;
using Environment = Digipost.Api.Client.Common.Environment;
using Sender = Digipost.Api.Client.Common.Sender;

namespace Digipost.Api.Client.Docs
{
    public class SendExamples
    {
        private static readonly ClientConfig ClientConfig = new ClientConfig(new Broker(123456), Environment.Production);
        private static readonly DigipostClient client = new DigipostClient(ClientConfig, thumbprint: "84e492a972b7e...");
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
            const int brokerId = 12345;
            
            var broker = new Broker(brokerId);
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
            var message = new Message(
                sender,
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxx"),
                new Invoice(
                    subject: "Invoice 1",
                    fileType: "pdf",
                    path: @"c:\...\invoice.pdf",
                    amount: new decimal(100.21),
                    account: "2593143xxxx",
                    duedate: DateTime.Parse("01.01.2016"),
                    kid: "123123123")
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
            var startTime = DateTime.Parse("2017-11-24T13:00:00+0100");
            var appointment = new Appointment
            {
                Start_Time = startTime.ToString("O"),
                End_Time = startTime.AddMinutes(30).ToString("O"),
                Address = new Address{ Street_Address = "Storgata 1", Postal_Code = "0001", City = "Oslo" }
            };

            string appointmentXml = SerializeUtil.Serialize(appointment);

            var document = new Document(
                subject: "Your appointment",
                fileType: "pdf",
                path: @"c:\...\document.pdf",
                dataType: appointmentXml
            );
        }
        
        public void SendMessageWithEventMetadata()
        {
            var startTime = DateTime.Parse("2017-11-24T13:00:00+0100");
            
            var timeInterval1 = new TimeInterval { Start_Time = DateTime.Today.ToString("O"), End_Time = DateTime.Today.AddHours(3).ToString("O") };
            var timeInterval2 = new TimeInterval { Start_Time = DateTime.Today.AddDays(1).ToString("O"), End_Time = DateTime.Today.AddDays(1).AddHours(5).ToString("O") };
            
            var barcode = new Barcode { Barcode_Value = "12345678", Barcode_Type = "insert type here", Barcode_Text = "this is a code", Show_Value_In_Barcode = true };
            var address = new Address { Street_Address = "Gateveien 1", Postal_Code = "0001", City = "Oslo" };
            var info = new Info { Title = "Title", Text = "Very important information" };
            var link = new Link { Url = "https://www.test.no", Description = "This is a link" };

            Event @event = new Event
            {
                Start_Time = { timeInterval1, timeInterval2 },
                Description = "Description here",
                Address = address,
                Info = { info },
                Place = "Oslo City Røntgen",
                PlaceLabel = "This is a place",
                Sub_Title = "SubTitle",
                Barcode = barcode,
                BarcodeLabel = "Barcode Label",
                Links = { link }
            };

            string eventXml = SerializeUtil.Serialize(@event);
            
            Document document = new Document(
                subject: "Your appointment",
                fileType: "pdf",
                path: @"c:\...\document.pdf",
                dataType: eventXml
            );
        }
    }
}
