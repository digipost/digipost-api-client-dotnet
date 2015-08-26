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
    public class MessageDataTransferObject
    {
        private MessageDataTransferObject()
        {
            /**Must exist for serialization.**/
        }

       public MessageDataTransferObject(RecipientDataTransferObject recipient, DocumentDataTransferObject primaryDocumentDataTransferObject)
        {
            Recipient = recipient;
            PrimaryDocumentDataTransferObject = primaryDocumentDataTransferObject;
            Attachments = new List<DocumentDataTransferObject>();
        }

       public MessageDataTransferObject(RecipientDataTransferObject recipient, DocumentDataTransferObject primaryDocumentDataTransferObject, string senderId):
            this(recipient, primaryDocumentDataTransferObject)
        {
            SenderId = senderId;
        }
        
        [XmlElement("sender-id")]
        public string SenderId { get; set; }

        [XmlElement("recipient")]
        public RecipientDataTransferObject Recipient { get; set; }

        [XmlElement("delivery-time")]
        public DateTime? DeliveryTime { get; set; }

        public bool DeliveryTimeSpecified
        {
            /* This method must be specified for serialization, and is connected by convention to DeliveryTime,
                so do not rename.
            */
            get { return DeliveryTime != null; }
        }
        
        [XmlElement("primary-document")]
        public DocumentDataTransferObject PrimaryDocumentDataTransferObject { get; set; }

        [XmlElement("attachment")]
        public List<DocumentDataTransferObject> Attachments { get; set; }
    }
}