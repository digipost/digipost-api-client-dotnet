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
        private static string DocumentGuid = Guid.NewGuid().ToString();//'[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}'
        private static string xmlMessage;
        private static byte[] attachment = GetAttachment();
        private static string technicalSenderId = "779052"; //106768801

        static void Main(string[] args)
        {
            xmlMessage = getXmlFromClass(DocumentGuid);
            var t = DigipostApi.Send(technicalSenderId, "forsendelseId", "Digipostadresse", "Emne", xmlMessage, attachment, DocumentGuid);
            var r = t.Result;
        }

        private static byte[] GetAttachment()
        {
            var path = @"\\vmware-host\Shared Folders\Development\Hoveddokument.txt";
            return File.ReadAllBytes(path);
        }

        private static string getXmlFromClass(String docId)
        {
            //primary document
            document doc = new document();
            doc.authenticationlevel =authenticationlevel.PASSWORD;
            doc.authenticationlevelSpecified = doc.authenticationlevel != null;
            doc.sensitivitylevel = sensitivitylevel.NORMAL;
            doc.sensitivitylevelSpecified  = doc.sensitivitylevel != null;
            
            doc.filetype = "txt";
            doc.subject = "test subject";
            doc.uuid = docId;
            
            //recipient
            messagerecipient mr = new messagerecipient();
            mr.ItemElementName = ItemChoiceType1.personalidentificationnumber;
            mr.Item = "31108446911";
            
            //message
            message m = new message();
            m.primarydocument = doc;
            m.recipient = mr;

            return SerializeUtil.Serialize(m);

        }

    }
}
