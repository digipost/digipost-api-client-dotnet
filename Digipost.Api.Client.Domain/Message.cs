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
        private Document[] _attachments;
        private Document _primarydocument;
        private MessageRecipient _recipient;

        /// <remarks />
        [XmlElement("recipient")]
        public MessageRecipient Recipient
        {
            get { return _recipient; }
            set { _recipient = value; }
        }

        /// <remarks />
        [XmlElement("primary-document")]
        public Document PrimaryDocument
        {
            get { return _primarydocument; }
            set { _primarydocument = value; }
        }

        /// <remarks />
        [XmlElement("attachment")]
        public Document[] Attachment
        {
            get { return _attachments; }
            set { _attachments = value; }
        }
    }
}