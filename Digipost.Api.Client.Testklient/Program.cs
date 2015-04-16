using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private const string TechnicalSenderId = "779052";  //"106768801";
        private static readonly ResourceUtility _resourceUtility = new ResourceUtility("Digipost.Api.Client.Testklient.Resources");

        private static void Main(string[] args)
        {
            var message = GetMessage();
            var config = new ClientConfig(TechnicalSenderId);
            Logging.Initialize(config);

            var api = new DigipostApi(config, GetCert());
            var t = api.Send(message);

            var r = t.Result;
            Console.ReadKey();
        }

        private static Message GetMessage()
        {
            //primary document
            var doc = new Document("Testsubject", "txt", GetPrimaryDocument());
            
            //recipient
            var mr = new MessageRecipient
            {
                IdentificationType = IdentificationChoice.PersonalidentificationNumber,
                IdentificationValue = "31108446911"
            };

            //message
            var m = new Message(mr, doc);
            return m;
        }

        private static byte[] GetPrimaryDocument()
        {
            return _resourceUtility.ReadAllBytes(true, "Hoveddokument.txt");

        }

        private static byte[] GetAttachment()
        {
            return _resourceUtility.ReadAllBytes(true, "Vedlegg.txt");
        }

        private static X509Certificate2 GetCert()
        {
            //‎f7 de 9c 38 4e e6 d0 a8 1d ad 7e 8e 60 bd 37 76 fa 5d e9 f4
            //var thumbprint = "‎‎‎84e492a972b7edc197a32d9e9c94ea27bd5ac4d9".ToUpper();
            string thumbprint = "84e492a972b7edc197a32d9e9c94ea27bd5ac4d9".ToUpper();

            return CertificateUtility.SenderCertificate(thumbprint, Language.English);
        }
    }
}