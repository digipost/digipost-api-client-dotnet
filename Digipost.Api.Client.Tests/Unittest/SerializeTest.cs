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
         [TestMethod]
         public void SerializeDocument()
         {
             var document = new Document("Subject", "txt", GetBytes("test"), AuthenticationLevel.TwoFactor,
                 SensitivityLevel.Sensitive) {SmsNotification = new SmsNotification(2)};

             var serialized = SerializeUtil.Serialize(document);
             Assert.IsNotNull(serialized);

             var deserializedDocument = SerializeUtil.Deserialize<Document>(serialized);

             document.ContentBytes = null; //setting contntBytes to null -  since this is not sent inline in the xml(and is not serialized)
             
             TestHelper.LookLikeEachOther(document,deserializedDocument);

         }

         [TestMethod]
         public void SerializeRecipient()
         {

             var printDetails = new PrintDetails(new PrintRecipient("Kristian Sæther Enge",new NorwegianAddress("0460","Oslo","Collettsgate 68","Leil h401","dør 2")),new PrintReturnAddress("Ola Digipost",new ForeignAddress(CountryIdentifier.Country, "SE","svenskegatan 1"," leil h101","pb 12","skuff 3")));
             var recipient =
                 new Recipient(
                     new RecipientByNameAndAddress("Kristian Sæther Enge", "0460", "Oslo", "Colletts gate 68"),printDetails);
             
             var serialized = SerializeUtil.Serialize(recipient);
             Assert.IsNotNull(serialized);

             

             var deserializedRecipient = SerializeUtil.Deserialize<Recipient>(serialized);

             TestHelper.LookLikeEachOther(recipient, deserializedRecipient);
         }


         [TestMethod]
         public void SerializeMessage()
         {

             var printDetails = new PrintDetails(new PrintRecipient("Kristian Sæther Enge", new NorwegianAddress("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnAddress("Ola Digipost", new ForeignAddress(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));
             var recipient =
                 new Recipient(
                     new RecipientByNameAndAddress("Kristian Sæther Enge", "0460", "Oslo", "Colletts gate 68"), printDetails);
             var document = new Document("Subject", "txt", GetBytes("test"), AuthenticationLevel.TwoFactor,
                 SensitivityLevel.Sensitive) { SmsNotification = new SmsNotification(2) };
             
             var attachment = new Document("attachment", "txt", GetBytes("test"), AuthenticationLevel.TwoFactor,
                 SensitivityLevel.Sensitive);
             
             var message = new Message(recipient,document);
             message.Attachments.Add(attachment);

             var serialized = SerializeUtil.Serialize(message);
             Assert.IsNotNull(serialized);

             var deserializedMessage = SerializeUtil.Deserialize<Message>(serialized);

            TestHelper.LookLikeEachOther(message, deserializedMessage);
         }



         static byte[] GetBytes(string str)
         {
             byte[] bytes = new byte[str.Length * sizeof(char)];
             Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
             return bytes;
         }

         static string GetString(byte[] bytes)
         {
             char[] chars = new char[bytes.Length / sizeof(char)];
             Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
             return new string(chars);
         }
    }
}
