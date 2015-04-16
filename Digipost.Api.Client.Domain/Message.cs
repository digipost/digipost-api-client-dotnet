using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Message
    {
        public Message()
        {
            Attachments = new Document[0];
        }
        
        [XmlElement("recipient")]
        public MessageRecipient Recipient { get; set; }

        [XmlElement("primary-document")]
        public Document PrimaryDocument { get; set; }

        [XmlElement("attachment")]
        public Document[] Attachments { get; set; }
    }
}