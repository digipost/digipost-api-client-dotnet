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

        private string messageidField;

        private object senderIdField;

        private MessageRecipient recipientField;

        private string invoicereferenceField;

        private Document primarydocumentField;

        private Document[] attachmentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("message-id")]
        public string MessageId
        {
            get
            {
                return this.messageidField;
            }
            set
            {
                this.messageidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sender-id", typeof(long))]
        [System.Xml.Serialization.XmlElementAttribute("sender-organization", typeof(SenderOrganization))]
        public object SenderId
        {
            get
            {
                return this.senderIdField;
            }
            set
            {
                this.senderIdField = value;
            }
        }

        /// <remarks/>
        public MessageRecipient recipient
        {
            get
            {
                return this.recipientField;
            }
            set
            {
                this.recipientField = value;
            }
        }

      //TODO: fix deliveryDateSpecified. KSE

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("invoice-reference")]
        public string InvoiceReference
        {
            get
            {
                return this.invoicereferenceField;
            }
            set
            {
                this.invoicereferenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("primary-document")]
        public Document PrimaryDocument
        {
            get
            {
                return this.primarydocumentField;
            }
            set
            {
                this.primarydocumentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attachment")]
        public Document[] Attachment
        {
            get
            {
                return this.attachmentField;
            }
            set
            {
                this.attachmentField = value;
            }
        }
    }
}
