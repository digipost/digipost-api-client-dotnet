using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest
{
    [TestClass]
    public class SerializeUtilTests
    {
        public IComparator Comparator = new Comparator();

        [TestClass]
        public class DeserializeMethod : SerializeUtilTests
        {
            [TestMethod]
            public void ReturnsProperDeserializedMessageWithInvoice()
            {
                //Arrange
                var messageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Ola Nordmann</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>1222222</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document><attachment xsi:type=""invoice""><uuid>123456</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level><kid>123123123123</kid><amount>100</amount><account>18941362738</account><due-date>2018-01-01</due-date></attachment></message>";
                var printDetails = new PrintDetailsDataTransferObject(new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnRecipientDataTransferObject("Ola Digipost", new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
                var recipient =
                    new RecipientDataTransferObject(
                        new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);
                var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "1222222", SmsNotification = new SmsNotificationDataTransferObject(2) };

                var attachment =  new InvoiceDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"),100,"18941362738",DateTime.Parse("2018-01-01"),"123123123123", AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "123456", SmsNotification = new SmsNotificationDataTransferObject(2) };


                var messageTemplate = new MessageDataTransferObject(recipient, document);
                messageTemplate.Attachments.Add(attachment);

                //Act
                var deserializedMessageBlueprint = SerializeUtil.Deserialize<MessageDataTransferObject>(messageBlueprint);
                document.ContentBytes = null;   //Bytes are not included as a part of XML (XmlIgnore)
                attachment.ContentBytes = null; //Bytes are not included as a part of XML (XmlIgnore)
                
                //Assert
                Assert.IsNull(deserializedMessageBlueprint.DeliveryTime);
                Comparator.AreEqual(messageTemplate, deserializedMessageBlueprint);
            }
            
            [TestMethod]
            public void ReturnProperDeserializedMessageWithDeliveryTime()
            {
                //Arrange
                const string messageWithDeliverytimeBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><personal-identification-number>00000000000</personal-identification-number></recipient><delivery-time>2015-07-27T00:00:00</delivery-time><primary-document><uuid>786711a5-1ed6-4f7c-8eda-a5b762c446cb</uuid><subject>Integrasjonstjest</subject><file-type>txt</file-type><authentication-level>PASSWORD</authentication-level><sensitivity-level>NORMAL</sensitivity-level></primary-document></message>";

                var messageWithDeliverytime = DomainUtility.GetSimpleMessage();
                messageWithDeliverytime.PrimaryDocument.Guid = "786711a5-1ed6-4f7c-8eda-a5b762c446cb"; //To ensure that the guid is the same as in the blueprint
                messageWithDeliverytime.DeliveryTime = new DateTime(2015, 07, 27);
                messageWithDeliverytime.PrimaryDocument.ContentBytes = null;

                //Act
                var deserializedMessageWithDeliverytime = SerializeUtil.Deserialize<MessageDataTransferObject>(messageWithDeliverytimeBlueprint);

                //Assert
               Comparator.AreEqual(messageWithDeliverytime, deserializedMessageWithDeliverytime);
            }


            [TestMethod]
            public void ReturnsProperDeserializedMessageWithSenderOrganizationId()
            {
                //Arrange
                var messageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><sender-id>1237732</sender-id><recipient><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Ola Nordmann</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>1222222</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document></message>";
                var printDetails = new PrintDetailsDataTransferObject(new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnRecipientDataTransferObject("Ola Digipost", new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
                var recipient =
                    new RecipientDataTransferObject(
                        new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);
                var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "1222222", SmsNotification = new SmsNotificationDataTransferObject(2) };

                var messageTemplate = new MessageDataTransferObject(recipient, document, "1237732");
                
                //Act
                var deserializedMessageBlueprint = SerializeUtil.Deserialize<MessageDataTransferObject>(messageBlueprint);
                document.ContentBytes = null;   //Bytes are not included as a part of XML (XmlIgnore)

                //Assert
               Comparator.AreEqual(messageTemplate, deserializedMessageBlueprint);
            }

            [TestMethod]
            public void ReturnProperDeserializedDocument()
            {
                //Arrange
                const string documentBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><uuid>123456</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></document>";
                var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "123456", SmsNotification = new SmsNotificationDataTransferObject(2) };

                //Act
                var deserializedDocumentBlueprint = SerializeUtil.Deserialize<DocumentDataTransferObject>(documentBlueprint);
                document.ContentBytes = null;    //Bytes are not included as a part of XML (XmlIgnore)

                //Assert
               Comparator.AreEqual(document, deserializedDocumentBlueprint);
            }

            [TestMethod]
            public void ReturnProperDerializedRecipient()
            {
                //Arrange
                const string recipientBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message-recipient xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Ola Nordmann</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></message-recipient>";
                var printDetails = new PrintDetailsDataTransferObject(new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnRecipientDataTransferObject("Ola Digipost", new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
                var recipient =
                    new RecipientDataTransferObject(
                        new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);

                //Act
                var deserializedRecipientBlueprint = SerializeUtil.Deserialize<RecipientDataTransferObject>(recipientBlueprint);

                //Assert
               Comparator.AreEqual(recipient, deserializedRecipientBlueprint);

            }

            [TestMethod]
            public void ReturnsProperDeserializedIdentificationByNameAndAddress()
            {
                //Arrange
                const string identificationBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><identification xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Postgirobygget 16</addressline1><postalcode>0001</postalcode><city>Oslo</city></name-and-address></identification>";
                var identification = new IdentificationDataTransferObject(new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Postgirobygget 16"));

                //Act
                var deserializedIdentificationBlueprint = SerializeUtil.Deserialize<IdentificationDataTransferObject>(identificationBlueprint);

                //Assert
               Comparator.AreEqual(identification, deserializedIdentificationBlueprint);
            }
            
            [TestMethod]
            public void ReturnsProperDeserializedIdentificationByDigipostAddress()
            {
                //Arrange
                const string identificationBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><identification xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><digipost-address>ola.nordmann#123abc</digipost-address></identification>";
                var identification = new IdentificationDataTransferObject(IdentificationChoiceType.DigipostAddress, "ola.nordmann#123abc");
                
                //Act
                var deserializedIdentificationBlueprint = SerializeUtil.Deserialize<IdentificationDataTransferObject>(identificationBlueprint);

                //Assert
               Comparator.AreEqual(identification, deserializedIdentificationBlueprint);
            }

            [TestMethod]
            public void ReturnsProperDeserializedInvoice()
            {
                //Arrange
                const string invoiceBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><invoice xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><uuid>123456</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level><kid>123123123123</kid><amount>100</amount><account>18941362738</account><due-date>2018-01-01</due-date></invoice>";
                var invoice = new InvoiceDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), 100, "18941362738", DateTime.Parse("2018-01-01"), "123123123123", AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "123456", SmsNotification = new SmsNotificationDataTransferObject(2) };

                //Act
                var deserializedInvoice = SerializeUtil.Deserialize<InvoiceDataTransferObject>(invoiceBlueprint);

                //Assert
               Comparator.AreEqual(invoice, deserializedInvoice);
            }
            
            [TestMethod]
            public void ReturnsProperDeserializedSearchResult()
            {
                //Arrange
                const string personDetailsResultBlueprint =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><recipients xmlns=\"http://api.digipost.no/schema/v6\"><recipient><firstname>aleksander</firstname><middlename></middlename><lastname>larsen</lastname><digipost-address>aleksander.larsen#XX22DD</digipost-address><mobile-number>45456565</mobile-number><organisation-name>organ-isasjonen</organisation-name><address><street>gronerlukkagata</street><house-number>47</house-number><additional-addressline>ekstrainfo</additional-addressline><zip-code>0475</zip-code><city>oslo</city></address><link rel=\"https://qa2.api.digipost.no/relations/self\" uri=\"https://qa2.api.digipost.no/recipients/jon.aleksander.aase%239PNU\" media-type=\"application/vnd.digipost-v6+xml\"/></recipient></recipients>";
                
                SearchDetailsResult searchDetailsResult = new SearchDetailsResult
                {
                    PersonDetails = new List<SearchDetails>
                    {
                        new SearchDetails
                        {
                            DigipostAddress = "aleksander.larsen#XX22DD", 
                            FirstName = "aleksander",
                            MiddleName = "",
                            LastName = "larsen", 
                            MobileNumber = "45456565",
                            OrganizationName = "organ-isasjonen",
                            SearchDetailsAddress = new SearchDetailsAddress
                            {
                                City = "oslo",
                                Street = "gronerlukkagata",
                                HouseNumber = "47",
                                ZipCode = "0475",
                                AdditionalAddressLine = "ekstrainfo"
                            }
                        }
                    }
                };

                //Act
                var deserializedPersonDetailsResultBlueprint = SerializeUtil.Deserialize<SearchDetailsResult>(personDetailsResultBlueprint);

                //Assert
               Comparator.AreEqual(searchDetailsResult, deserializedPersonDetailsResultBlueprint);
            }
        }

        [TestClass]
        public class SerializeMethod : SerializeUtilTests
        {
            [TestMethod]
            public void ReturnProperSerializedMessageWithInvoice()
            {
                //Arrange
                const string messageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Ola Nordmann</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>03fdf738-5c7e-420d-91e2-2b95e025d635</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document><attachment><uuid>c75d8cb9-bd47-4ae7-9bd3-92105d12f49a</uuid><subject>attachment</subject><file-type>txt</file-type><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></attachment></message>";
                var printDetails = new PrintDetailsDataTransferObject(new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnRecipientDataTransferObject("Ola Digipost", new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
                var recipient =
                    new RecipientDataTransferObject(
                        new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);
                var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { SmsNotification = new SmsNotificationDataTransferObject(2), Guid = "03fdf738-5c7e-420d-91e2-2b95e025d635" };
                var attachment = new DocumentDataTransferObject("attachment", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "c75d8cb9-bd47-4ae7-9bd3-92105d12f49a" };
                var message = new MessageDataTransferObject(recipient, document);
                message.Attachments.Add(attachment);

                //Act
                var serialized = SerializeUtil.Serialize(message);
                
                //Assert
                Assert.IsNotNull(serialized);
                Assert.AreEqual(messageBlueprint, serialized);

            }
            
            [TestMethod]
            public void ReturnsProperSerializedMessageWithSenderOrganizationId()
            {
                //Arrange
                var messageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><sender-id>1237732</sender-id><recipient><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Ola Nordmann</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>1222222</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document></message>";
                var printDetails = new PrintDetailsDataTransferObject(new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnRecipientDataTransferObject("Ola Digipost", new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
                var recipient =
                    new RecipientDataTransferObject(
                        new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);
                var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "1222222", SmsNotification = new SmsNotificationDataTransferObject(2) };

                var messageTemplate = new MessageDataTransferObject(recipient, document, "1237732");

                //Act
                var serializedMessage = SerializeUtil.Serialize(messageTemplate);
          
                //Assert
                Assert.IsNotNull(serializedMessage);
                Assert.AreEqual(messageBlueprint, serializedMessage);
            }

            [TestMethod]
            public void ReturnProperSerializedMessageWithDeliveryTime()
            {
                //Arrange
                const string messageWithDeliverytimeBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><personal-identification-number>00000000000</personal-identification-number></recipient><delivery-time>2015-07-27T00:00:00</delivery-time><primary-document><uuid>786711a5-1ed6-4f7c-8eda-a5b762c446cb</uuid><subject>Integrasjonstjest</subject><file-type>txt</file-type><authentication-level>PASSWORD</authentication-level><sensitivity-level>NORMAL</sensitivity-level></primary-document></message>";

                var simpleMessage = DomainUtility.GetSimpleMessage();
                simpleMessage.PrimaryDocument.Guid = "786711a5-1ed6-4f7c-8eda-a5b762c446cb"; //To ensure that the guid is the same as in the blueprint
                var messageWithDeliverytime = DataTransferObjectConverter.ToDataTransferObject(simpleMessage);
                messageWithDeliverytime.DeliveryTime = new DateTime(2015, 07, 27);

                //Act
                var serializedMessageWithDeliverytime = SerializeUtil.Serialize(messageWithDeliverytime);

                //Assert
                Assert.IsNotNull(serializedMessageWithDeliverytime);
                Assert.AreEqual(messageWithDeliverytimeBlueprint, serializedMessageWithDeliverytime);
            }
            
            [TestMethod]
            public void ReturnProperSerializedDocument()
            {
                //Arrange
                const string documentBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><uuid>123456</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></document>";
                var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "123456", SmsNotification = new SmsNotificationDataTransferObject(2) };

                //Act
                var serialized = SerializeUtil.Serialize(document);

                //Assert
                Assert.IsNotNull(serialized);
                Assert.AreEqual(documentBlueprint, serialized);
            }

            [TestMethod]
            public void ReturnProperSerializedRecipient()
            {
                //Arrange
                const string recipientBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message-recipient xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Ola Nordmann</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></message-recipient>";
                var printDetails = new PrintDetailsDataTransferObject(new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnRecipientDataTransferObject("Ola Digipost", new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
                var recipient =
                    new RecipientDataTransferObject(
                        new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);

                //Act
                var serialized = SerializeUtil.Serialize(recipient);
                
                //Assert
                Assert.IsNotNull(serialized);
                Assert.AreEqual(recipientBlueprint, serialized);

            }

            [TestMethod]
            public void ReturnsProperSerializedIdentificationByNameAndAddress()
            {
                //Arrange
                const string identificationBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><identification xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><name-and-address><fullname>Ola Nordmann</fullname><addressline1>Postgirobygget 16</addressline1><postalcode>0001</postalcode><city>Oslo</city></name-and-address></identification>";
                var identification = new IdentificationDataTransferObject(new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Postgirobygget 16"));

                //Act
                var serializedIdentificationBlueprint = SerializeUtil.Serialize(identification);

                //Assert
                Assert.IsNotNull(serializedIdentificationBlueprint);
                Assert.AreEqual(identificationBlueprint, serializedIdentificationBlueprint);
            }

            [TestMethod]
            public void ReturnsProperSerializedIdentificationByDigipostAddress()
            {
                //Arrange
                const string identificationBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><identification xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><digipost-address>ola.nordmann#123abc</digipost-address></identification>";
                var identification = new IdentificationDataTransferObject(IdentificationChoiceType.DigipostAddress, "ola.nordmann#123abc");

                //Act
                var serializedIdentification = SerializeUtil.Serialize(identification);

                //Assert
                Assert.IsNotNull(serializedIdentification);
                Assert.AreEqual(identificationBlueprint, serializedIdentification);
            }

            [TestMethod]
            public void ReturnProperSerializedInvoice()
            {
                //Arrange
                const string invoiceBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><invoice xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><uuid>123456</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level><kid>123123123123</kid><amount>100</amount><account>18941362738</account><due-date>2018-01-01</due-date></invoice>";
                var invoice = new InvoiceDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"),100,"18941362738",DateTime.Parse("2018-01-01"),"123123123123", AuthenticationLevel.TwoFactor,
                    SensitivityLevel.Sensitive) { Guid = "123456", SmsNotification = new SmsNotificationDataTransferObject(2) };

                //Act
                var serializedIdentification = SerializeUtil.Serialize(invoice);

                //Assert
                Assert.IsNotNull(serializedIdentification);
                Assert.AreEqual(invoiceBlueprint, serializedIdentification);
            }

            [TestMethod]
            public void ReturnsProperSerializedPeronDetailsResult()
            {
                //Arrange
                const string personDetailsResultBlueprint =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?><recipients xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://api.digipost.no/schema/v6\"><recipient><firstname>aleksander</firstname><middlename>mellan</middlename><lastname>larsen</lastname><digipost-address>aleksander.larsen#XX22DD</digipost-address><mobile-number>45456565</mobile-number><organisation-name>organ-isasjonen</organisation-name><address><street>gronerlukkagata</street><house-number>47</house-number><additional-addressline>ekstrainfo</additional-addressline><zip-code>0475</zip-code><city>oslo</city></address></recipient></recipients>";
                     
                SearchDetailsResult searchDetailsResult = new SearchDetailsResult
                {
                    PersonDetails = new List<SearchDetails>
                    {
                        new SearchDetails
                        {
                            DigipostAddress = "aleksander.larsen#XX22DD", 
                            FirstName = "aleksander",
                            MiddleName = "mellan",
                            LastName = "larsen", 
                            MobileNumber = "45456565",
                            OrganizationName = "organ-isasjonen",
                            SearchDetailsAddress = new SearchDetailsAddress
                            {
                                City = "oslo",
                                Street = "gronerlukkagata",
                                HouseNumber = "47",
                                ZipCode = "0475",
                                AdditionalAddressLine = "ekstrainfo"
                            }
                        }
                    }
                };

                //Act
                var serializedPersonDetailsResult = SerializeUtil.Serialize(searchDetailsResult);

                //Assert
                Assert.AreEqual(personDetailsResultBlueprint, serializedPersonDetailsResult);
            }
        }

    }
}
