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
        private static string xmlMessage;
        private static byte[] attachment;
        private static string DocumentGuid = Guid.NewGuid().ToString();

        static void Main(string[] args)
        {

            xmlMessage = GetXmlMessage();
            attachment = GetAttachment();
            var t = DigipostApi.Send("106768801", "forsendelseId", "Digipostadresse", "Emne", xmlMessage, attachment, DocumentGuid);
            var r = t.Result;
        }

        private static byte[] GetAttachment()
        {
            var path = @"\\vmware-host\Shared Folders\Development\Hoveddokument.txt";
            return File.ReadAllBytes(path);
        }

        private static string GetXmlMessage()
        {

            return
                string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><message xmlns=\"http://api.digipost.no/schema/v6\"><recipient>" +
                              "<personal-identification-number>01013300001</personal-identification-number></recipient>" +
                              "<primary-document><uuid>{0}</uuid><subject>Dokumentets emne</subject><file-type>txt</file-type><sms-notification><after-hours>1</after-hours></sms-notification><authentication-level>PASSWORD</authentication-level><sensitivity-level>NORMAL</sensitivity-level>" +
                              "</primary-document></message>", DocumentGuid);
        }
    }
}
