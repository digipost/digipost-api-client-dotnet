using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "message", Namespace = "http://api.digipost.no/schema/v6")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public partial class Message
    {

        private MessageRecipient recipient;

        private Document primarydocument;

        private Document[] attachments;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("recipient")]
        public MessageRecipient Recipient
        {
            get
            {
                return this.recipient;
            }
            set
            {
                this.recipient = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("primary-document")]
        public Document PrimaryDocument
        {
            get
            {
                return this.primarydocument;
            }
            set
            {
                this.primarydocument = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attachment")]
        public Document[] Attachment
        {
            get
            {
                return this.attachments;
            }
            set
            {
                this.attachments = value;
            }
        }
    }
}
