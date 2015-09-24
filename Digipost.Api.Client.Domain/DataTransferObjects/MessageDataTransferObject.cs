using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.DataTransferObjects
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

       public MessageDataTransferObject(RecipientDataTransferObject recipientDataTransferObject, DocumentDataTransferObject primaryDocumentDataTransferObject)
        {
            RecipientDataTransferObject = recipientDataTransferObject;
            PrimaryDocumentDataTransferObject = primaryDocumentDataTransferObject;
            Attachments = new List<DocumentDataTransferObject>();
        }

       public MessageDataTransferObject(RecipientDataTransferObject recipientDataTransferObject, DocumentDataTransferObject primaryDocumentDataTransferObject, string senderId):
            this(recipientDataTransferObject, primaryDocumentDataTransferObject)
        {
            SenderId = senderId;
        }
        
        [XmlElement("sender-id")]
        public string SenderId { get; set; }

        [XmlElement("recipient")]
        public RecipientDataTransferObject RecipientDataTransferObject { get; set; }

        [XmlElement("delivery-time")]
        public DateTime? DeliveryTime { get; set; }

        public bool DeliveryTimeSpecified
        {
            /* This method must be specified for serialization, and is connected by convention to DeliveryTime,
                so do not rename.            */
            get { return DeliveryTime != null; }
        }
        
        [XmlElement("primary-document")]
        public DocumentDataTransferObject PrimaryDocumentDataTransferObject { get; set; }

        [XmlElement("attachment")]
        public List<DocumentDataTransferObject> Attachments { get; set; }
    }
}