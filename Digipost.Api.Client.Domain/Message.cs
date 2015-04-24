using System;
using System.CodeDom.Compiler;
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
    public class Message
    {
        private Message(){ /**Must exist for serialization.**/ }

        public Message(MessageRecipient messageRecipient, Document primaryDocument)
        {
           Recipient = messageRecipient;
           PrimaryDocument = primaryDocument;
           Attachments = new List<Document>();
        }

        [XmlElement("recipient")]
        public MessageRecipient Recipient { get; set; }

        [XmlElement("primary-document")]
        public Document PrimaryDocument { get; set; }

        [XmlElement("attachment")]
        public List<Document> Attachments { get; set; }
    }
}