using System;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest
{
     [TestClass]
    public class SerializeTest
    {
         private const string DocumentBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><uuid>123456</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></document>";

         [TestMethod]
         public void SerializeDocument()
         {
             var document = new Document("Subject", "txt", TestHelper.GetBytes("test"), AuthenticationLevel.TwoFactor,
                 SensitivityLevel.Sensitive) {Guid = "123456" , SmsNotification = new SmsNotification(2)};

             var serialized = SerializeUtil.Serialize(document);
             Assert.IsNotNull(serialized);
             Assert.AreEqual(DocumentBlueprint,serialized);
             
         }

         private const string InvoiceBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><invoice xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><uuid>123456</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level><kid>123</kid><amount>100</amount><account>123</account><due-date>2015-05-22</due-date></invoice>";
         [TestMethod]
         public void SerializeInvoice()
         {
             var document = new Invoice("Subject", "txt", TestHelper.GetBytes("test"), AuthenticationLevel.TwoFactor,
                 SensitivityLevel.Sensitive) { Guid = "123456", SmsNotification = new SmsNotification(2),Kid = "123",Duedate = DateTime.Now ,Amount = 100,Account = "123" };

             var serialized = SerializeUtil.Serialize(document);
             Assert.IsNotNull(serialized);
             Assert.AreEqual(InvoiceBlueprint, serialized);

         }

         private const string RecipientBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message-recipient xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><name-and-address><fullname>Kristian Sæther Enge</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Kristian Sæther Enge</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></message-recipient>";
         [TestMethod]
         public void SerializeRecipient()
         {

             var printDetails = new PrintDetails(new PrintRecipient("Kristian Sæther Enge",new NorwegianAddress("0460","Oslo","Collettsgate 68","Leil h401","dør 2")),new PrintReturnAddress("Ola Digipost",new ForeignAddress(CountryIdentifier.Country, "SE","svenskegatan 1"," leil h101","pb 12","skuff 3")));
             var recipient =
                 new Recipient(
                     new RecipientByNameAndAddress("Kristian Sæther Enge", "0460", "Oslo", "Colletts gate 68"),printDetails);
             
             var serialized = SerializeUtil.Serialize(recipient);
             Assert.IsNotNull(serialized);
             Assert.AreEqual(RecipientBlueprint,serialized);

         }


         private const string MessageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><name-and-address><fullname>Kristian Sæther Enge</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Kristian Sæther Enge</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>03fdf738-5c7e-420d-91e2-2b95e025d635</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document><attachment><uuid>c75d8cb9-bd47-4ae7-9bd3-92105d12f49a</uuid><subject>attachment</subject><file-type>txt</file-type><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></attachment></message>";
         [TestMethod]
         public void SerializeMessage()
         {

             var printDetails = new PrintDetails(new PrintRecipient("Kristian Sæther Enge", new NorwegianAddress("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnAddress("Ola Digipost", new ForeignAddress(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
             var recipient =
                 new Recipient(
                     new RecipientByNameAndAddress("Kristian Sæther Enge", "0460", "Oslo", "Colletts gate 68"), printDetails);
             var document = new Document("Subject", "txt", TestHelper.GetBytes("test"), AuthenticationLevel.TwoFactor,
                 SensitivityLevel.Sensitive) { SmsNotification = new SmsNotification(2), Guid = "03fdf738-5c7e-420d-91e2-2b95e025d635" };
             
             var attachment = new Document("attachment", "txt", TestHelper.GetBytes("test"), AuthenticationLevel.TwoFactor,
                 SensitivityLevel.Sensitive) { Guid = "c75d8cb9-bd47-4ae7-9bd3-92105d12f49a" };
             
             var message = new Message(recipient,document);
             message.Attachments.Add(attachment);

             var serialized = SerializeUtil.Serialize(message);
             Assert.IsNotNull(serialized);
             Assert.AreEqual(MessageBlueprint,serialized);

         }
    }
}
