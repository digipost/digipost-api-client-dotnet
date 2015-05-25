using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest
{
    [TestClass]
    public class DeserializeTests
    {
        private const string MessageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><name-and-address><fullname>Kristian Sæther Enge</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Kristian Sæther Enge</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>1222222</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document><attachment><uuid>123456</uuid><subject>attachment</subject><file-type>txt</file-type><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></attachment></message>";
        
        [TestMethod]
        public void DeserializeMessage()
        {

            var printDetails = new PrintDetails(new PrintRecipient("Kristian Sæther Enge", new NorwegianAddress("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnAddress("Ola Digipost", new ForeignAddress(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
            var recipient =
                new Recipient(
                    new RecipientByNameAndAddress("Kristian Sæther Enge", "0460", "Oslo", "Colletts gate 68"), printDetails);
            var document = new Document("Subject", "txt", TestHelper.GetBytes("test"), AuthenticationLevel.TwoFactor,
                SensitivityLevel.Sensitive) { Guid = "1222222", SmsNotification = new SmsNotification(2) };

            var attachment = new Document("attachment", "txt",TestHelper.GetBytes("test"), AuthenticationLevel.TwoFactor,
                SensitivityLevel.Sensitive){Guid = "123456"};
            

            var message = new Message(recipient, document);
            message.Attachments.Add(attachment);
            
            var serialized = SerializeUtil.Serialize(message);
            Assert.IsNotNull(serialized);

            var deserializedMessage = SerializeUtil.Deserialize<Message>(serialized);

            var deserializedMessageBlueprint = SerializeUtil.Deserialize<Message>(MessageBlueprint);

            TestHelper.LookLikeEachOther(deserializedMessageBlueprint, deserializedMessage);
        }
    }
}
