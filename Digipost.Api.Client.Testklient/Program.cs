using Digipost.Api.Client.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Testklient
{
    class Program
    {
        private static string technicalSenderId = "779052"; //106768801
        

        static void Main(string[] args)
        {
            Message message = GetMessage();
            var t = DigipostApi.Send(technicalSenderId,  message);
            var r = t.Result;
            
        }

        private static Message GetMessage()
        {
            //primary document
            Document doc = new Document();
            doc.Authenticationlevel =AuthenticationLevel.PASSWORD;
            doc.Sensitivitylevel = SensitivityLevel.NORMAL;
            doc.FileType = "pdf";
            doc.Subject = "test";
            doc.Uuid = Guid.NewGuid().ToString();
            doc.Content = GetPrimaryDocument();
            
            
            //attachment1
            Document attachment1 = new Document();
            attachment1.Authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment1.Sensitivitylevel = SensitivityLevel.NORMAL;
            attachment1.FileType = "pdf";
            attachment1.Subject = "attachment";
            attachment1.Uuid = Guid.NewGuid().ToString();
            attachment1.Content = GetAttachment();

            //attachment2
            Document attachment2 = new Document();
            attachment2.Authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment2.Sensitivitylevel = SensitivityLevel.NORMAL;
            attachment2.FileType = "pdf";
            attachment2.Subject = "attachment";
            attachment2.Uuid = Guid.NewGuid().ToString();
            attachment2.Content = GetAttachment();
            
            //recipient
            MessageRecipient mr = new MessageRecipient();
            mr.ItemElementName = IdentificationChoice.personalidentificationnumber;
            mr.IdentificationValue = "31108446911";

            //message
            Message m = new Message();
            
            m.PrimaryDocument = doc;
            m.Attachment = new Document[2];
            m.Attachment[0] = attachment1;
            m.Attachment[1] = attachment2;
            m.Recipient = mr;
            


            return m;

        }

        private static byte[] GetPrimaryDocument()
        {
            var documentPath = @"\\vmware-host\Shared Folders\Development\01_HelloWorld.pdf";
            return File.ReadAllBytes(documentPath);
        }
        
        private static byte[] GetAttachment()
        {
            var documentPath = @"\\vmware-host\Shared Folders\Development\04_HelloWorld.pdf";
            return File.ReadAllBytes(documentPath);
        }

    }
}
