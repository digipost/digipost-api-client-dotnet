using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Message : RequestContent
    {
        private Message()
        {
            /**Must exist for serialization.**/
        }

        public Message(Recipient recipient, Document primaryDocument)
        {
            Recipient = recipient;
            PrimaryDocument = primaryDocument;
            Attachments = new List<Document>();
        }

        [XmlElement("recipient")]
        public Recipient Recipient { get; set; }

        [XmlElement("primary-document")]
        public Document PrimaryDocument { get; set; }

        [XmlElement("attachment")]
        public List<Document> Attachments { get; set; }
    }
}