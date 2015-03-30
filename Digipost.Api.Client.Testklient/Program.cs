using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Testklient
{
    class Program
    {
        private static string DocumentGuid = Guid.NewGuid().ToString();//'[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}'
        private static string xmlMessage = GetXmlMessage(DocumentGuid);
        private static byte[] attachment = GetAttachment();
        private static string technicalSenderId = "779052"; //106768801

        static void Main(string[] args)
        {
 
            var t = DigipostApi.Send(technicalSenderId, "forsendelseId", "Digipostadresse", "Emne", xmlMessage, attachment, DocumentGuid);
            var r = t.Result;
        }

        private static byte[] GetAttachment()
        {
            var path = @"\\vmware-host\Shared Folders\Development\Hoveddokument.txt";
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
