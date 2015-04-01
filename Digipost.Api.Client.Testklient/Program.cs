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
            try { 
                var t = DigipostApi.Send(technicalSenderId,  message);
                var r = t.Result;
            }
            catch (Exception e)
            {
                String msg = e.Message;
            }
        }

        private static Message GetMessage()
        {
            //primary document
            Document doc = new Document();
            doc.authenticationlevel =AuthenticationLevel.PASSWORD;
            doc.authenticationlevelSpecified = doc.authenticationlevel != null;
            doc.sensitivitylevel = SensitivityLevel.NORMAL;
            doc.sensitivitylevelSpecified  = doc.sensitivitylevel != null;
            doc.filetype = "pdf";
            doc.subject = "test";
            doc.uuid = Guid.NewGuid().ToString();
            doc.contentOfDocument = GetPrimaryDocument();
            
            
            //attachment1
            Document attachment1 = new Document();
            attachment1.authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment1.sensitivitylevel = SensitivityLevel.NORMAL;
            attachment1.filetype = "pdf";
            attachment1.subject = "attachment";
            attachment1.uuid = Guid.NewGuid().ToString();
            attachment1.contentOfDocument = GetAttachment();

            //attachment2
            Document attachment2 = new Document();
            attachment2.authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment2.sensitivitylevel = SensitivityLevel.NORMAL;
            attachment2.filetype = "pdf";
            attachment2.subject = "attachment";
            attachment2.uuid = Guid.NewGuid().ToString();
            attachment2.contentOfDocument = GetAttachment();
            
            //recipient
            MessageRecipient mr = new MessageRecipient();
            mr.ItemElementName = IdentificationChoice.personalidentificationnumber;
            mr.Item = "31108446911";


            /*print details
            NorwegianAddress printAddress = new NorwegianAddress();
            printAddress.addressline1 = "Collettsgate 68";
            printAddress.addressline2 = "Leil. 0401";
            printAddress.city = "Oslo";
            printAddress.zipcode = "0460";

            PrintRecipient pr = new PrintRecipient();
            pr.name = "Kristian Sæther Enge";
            pr.Item = printAddress;

            PrintDetails pd = new PrintDetails();
            pd.nondeliverablehandling = NondeliverableHandling.SHRED;
            pd.posttype = PostType.B;
            pd.color = PrintColors.MONOCHROME;
            pd.recipient = pr;

            mr.printdetails = pd;
            */
            //message
            Message m = new Message();
            
            m.PrimaryDocument = doc;
            m.Attachment = new Document[2];
            m.Attachment[0] = attachment1;
            m.Attachment[1] = attachment2;
            m.recipient = mr;
            


            return m;

        }

        private static byte[] GetPrimaryDocument()
        {
            var documentPath = @"C:\Users\krist\Documents\GitHub\test.pdf";
            return File.ReadAllBytes(documentPath);
        }
        
        private static byte[] GetAttachment()
        {
            var documentPath = @"C:\Users\krist\Documents\GitHub\attachment.pdf";
            return File.ReadAllBytes(documentPath);
        }

    }
}
