using System;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
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
                 SensitivityLevel.Sensitive) {SmsNotification = new SmsNotification(DateTime.Now)};

             document.SmsNotification.AddAfterHours.Add(2); 

             var serialized = SerializeUtil.Serialize(document);
             Assert.IsNotNull(serialized);

             var deserializedDocument = SerializeUtil.Deserialize<Document>(serialized);

             document.ContentBytes = null; //setting contntBytes to null -  since this is not sent inline in the xml(and is not serialized)

             TestHelper.LookLikeEachOther(document,deserializedDocument);

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
