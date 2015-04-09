using System;
using System.IO;
using Digipost.Api.Client.Digipost.Api.Client;
using Digipost.Api.Client.Domain;
using System.Security.Cryptography.X509Certificates;
using DigipostApiClientShared;
using DigipostApiClientShared.Enums;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private static readonly string technicalSenderId = "106768801";

        private static void Main(string[] args)
        {
            var message = GetMessage();
            var config = new ClientConfig(technicalSenderId);
            Logging.Initialize(config);

            var api = new DigipostApi(config,GetCert());
            var t = api.Send(message);

            var r = t.Result;
            Console.ReadKey();
        }

        private static Message GetMessage()
        {
            //primary document
            var doc = new Document();
            doc.Authenticationlevel = AuthenticationLevel.PASSWORD;
            doc.Sensitivitylevel = SensitivityLevel.Normal;
            doc.FileType = "pdf";
            doc.Subject = "test";
            doc.Uuid = Guid.NewGuid().ToString();
            doc.Content = GetPrimaryDocument();


            //attachment1
            var attachment1 = new Document();
            attachment1.Authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment1.Sensitivitylevel = SensitivityLevel.Normal;
            attachment1.FileType = "pdf";
            attachment1.Subject = "attachment";
            attachment1.Uuid = Guid.NewGuid().ToString();
            attachment1.Content = GetAttachment();

            //attachment2
            var attachment2 = new Document();
            attachment2.Authenticationlevel = AuthenticationLevel.PASSWORD;
            attachment2.Sensitivitylevel = SensitivityLevel.Normal;
            attachment2.FileType = "pdf";
            attachment2.Subject = "attachment";
            attachment2.Uuid = Guid.NewGuid().ToString();
            attachment2.Content = GetAttachment();

            //recipient
            var mr = new MessageRecipient();
            mr.ItemElementName = IdentificationChoice.Personalidentificationnumber;
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
            var documentPath = @"\\vmware-host\Shared Folders\Development\01_HelloWorld.pdf";
            return File.ReadAllBytes(documentPath);
        }

        private static byte[] GetAttachment()
        {
            var documentPath = @"\\vmware-host\Shared Folders\Development\04_HelloWorld.pdf";
            return File.ReadAllBytes(documentPath);
        }

        private static X509Certificate2 GetCert()
        {
            var storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);
            const string thumbprint = "F7DE9C384EE6D0A81DAD7E8E60BD3776FA5DE9F4";

            return CertificateUtility.SenderCertificate(thumbprint, Language.English);
        }
    }
}