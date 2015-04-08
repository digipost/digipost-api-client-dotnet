using System;
using System.IO;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private static readonly string technicalSenderId = "106768801";

        private static void Main(string[] args)
        {
            var message = GetMessage();

            var config = new ClientConfig(technicalSenderId);
            var api = new DigipostApi(config);
            var t = api.Send(message);

            var r = t.Result;
        }

        private static Message GetMessage()
        {
            //primary document
            var doc = new Document();
            doc.Authenticationlevel = AuthenticationLevel.PASSWORD;
            doc.Sensitivitylevel = SensitivityLevel.NORMAL;
            doc.FileType = "pdf";
            doc.Subject = "test";
            doc.Uuid = Guid.NewGuid().ToString();
            doc.Content = GetPrimaryDocument();


            //attachment1
            var attachment1 = new Document();
            attachment1.Authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment1.Sensitivitylevel = SensitivityLevel.NORMAL;
            attachment1.FileType = "pdf";
            attachment1.Subject = "attachment";
            attachment1.Uuid = Guid.NewGuid().ToString();
            attachment1.Content = GetAttachment();

            //attachment2
            var attachment2 = new Document();
            attachment2.Authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment2.Sensitivitylevel = SensitivityLevel.NORMAL;
            attachment2.FileType = "pdf";
            attachment2.Subject = "attachment";
            attachment2.Uuid = Guid.NewGuid().ToString();
            attachment2.Content = GetAttachment();

            //recipient
            var mr = new MessageRecipient();
            mr.ItemElementName = IdentificationChoice.personalidentificationnumber;
            mr.Identification = "01013300001";

            //message
            var m = new Message();
            m.PrimaryDocument = doc;
            m.Attachment = new Document[2];
            m.Attachment[0] = attachment1;
            m.Attachment[1] = attachment2;
            m.Recipient = mr;


            return m;
        }

        private static byte[] GetPrimaryDocument()
        {
            var documentPath = @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\testdata\hoveddokument\OWASP_Top_10_ 2013.pdf";
            return File.ReadAllBytes(documentPath);
        }

        private static byte[] GetAttachment()
        {
            var documentPath = @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\testdata\hoveddokument\OWASP_Top_10_ 2013.pdf";
            return File.ReadAllBytes(documentPath);
        }
    }
}