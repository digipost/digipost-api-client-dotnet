using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    /// <summary>
    /// The response object that will be returned for every OK http request(200-299)
    /// </summary>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-delivery", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-delivery", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class MessageDeliveryResultDataTransferObject
    {
        [XmlElement("delivery-method")]
        public DeliveryMethod DeliveryMethod { get; set; }
        
        [XmlElement("status")]
        public MessageStatus Status { get; set; }

        [XmlElement("delivery-time")]
        public DateTime DeliveryTime { get; set; }

        [XmlElement("primary-document")]
        public DocumentDataTransferObject PrimaryDocumentDataTransferObject { get; set; }

        [XmlElement("attachment")]
        public List<DocumentDataTransferObject> AttachmentsDataTransferObject { get; set; }

        [XmlElement("link")]
        [Obsolete("This field has no relevant information and will therefore be removed in future version.")]
        public List<Link> Link { get; set; }

        public override string ToString()
        {
            var attachments = AttachmentsDataTransferObject.Aggregate(" ", (current, doc) => current + (doc.ToString() ));
            return
                string.Format(
                    "Deliverymethod: {0}, Status: {1}, DeliveryTime: {2}, Primarydocument: {3}, " +
                    "Attachment: {4}", DeliveryMethod, Status, DeliveryTime, PrimaryDocumentDataTransferObject,
                    attachments);
        }
    }
}