using System;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace Digipost.Api.Client.Testklient
{
    class Program
    {
        private static readonly string DocumentGuid = Guid.NewGuid().ToString();//'[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}'
        private static readonly string XmlMessage = GetXmlMessage(DocumentGuid);
        private const string TechnicalSenderId = "106768801";

        static void Main(string[] args)
        {
            var config = new ClientConfig(TechnicalSenderId);
            var api = new DigipostApi(config);

            var attachment = GetAttachment();

            var t = api.Send(TechnicalSenderId, "forsendelseId", "Digipostadresse", "Emne", XmlMessage, attachment, DocumentGuid);
            var r = t.Result;
        }

        private static byte[] GetAttachment()
        {
            var path =
                @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\testdata\hoveddokument\Hoveddokument.txt";
            return File.ReadAllBytes(path);
        }

        private static string GetXmlMessage(String docID)
        {

            return
                string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><message xmlns=\"http://api.digipost.no/schema/v6\"><recipient>" +
                              "<personal-identification-number>31108446911</personal-identification-number></recipient>" +
                              "<primary-document><uuid>{0}</uuid><subject>Dokumentets emne</subject><file-type>txt</file-type><sms-notification/><authentication-level>PASSWORD</authentication-level><sensitivity-level>NORMAL</sensitivity-level>" +
                              "</primary-document></message>", docID);
        }
    }
}
