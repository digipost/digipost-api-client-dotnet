using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Digipost.Api.Client;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private const string TechnicalSenderId = "106768801";
        private static ResourceUtility _resourceUtility = new ResourceUtility("Digipost.Api.Client.Testklient.Resources");

        private static void Main(string[] args)
        {
            var message = GetMessage();
            var config = new ClientConfig(TechnicalSenderId);
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
            doc.Authenticationlevel = AuthenticationLevel.Password;
            doc.Sensitivitylevel = SensitivityLevel.Normal;
            doc.FileType = "pdf";
            doc.Subject = "test";
            doc.Uuid = Guid.NewGuid().ToString();
            doc.Content = GetPrimaryDocument();


            //attachment1
            var attachment1 = new Document();
            attachment1.Authenticationlevel = AuthenticationLevel.Password;
            attachment1.Sensitivitylevel = SensitivityLevel.Normal;
            attachment1.FileType = "pdf";
            attachment1.Subject = "attachment";
            attachment1.Uuid = Guid.NewGuid().ToString();
            attachment1.Content = GetAttachment();

            //attachment2
            var attachment2 = new Document();
            attachment2.Authenticationlevel = AuthenticationLevel.Password;
            attachment2.Sensitivitylevel = SensitivityLevel.Normal;
            attachment2.FileType = "pdf";
            attachment2.Subject = "attachment";
            attachment2.Uuid = Guid.NewGuid().ToString();
            attachment2.Content = GetAttachment();

            //recipient
            var mr = new MessageRecipient();
            mr.IdentificationType = IdentificationChoice.PersonalidentificationNumber;
            mr.IdentificationValue = "01013300001";

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
            return _resourceUtility.ReadAllBytes(true ,"Hoveddokument.txt");

        }

        private static byte[] GetAttachment()
        {
            return _resourceUtility.ReadAllBytes(true, "Vedlegg.txt");
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