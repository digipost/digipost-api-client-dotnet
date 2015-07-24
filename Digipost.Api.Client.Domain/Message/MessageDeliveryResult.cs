using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    /// <summary>
    /// The response object that will be returned for every OK http request(200-299)
    /// </summary>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-delivery", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-delivery", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class MessageDeliveryResult
    {
        /// <summary>
        /// The type of delivery method
        /// </summary>
        [XmlElement("delivery-method")]
        public DeliveryMethod Deliverymethod { get; set; }

        /// <summary>
        /// The current status of the message in Digipost
        /// </summary>
        [XmlElement("status")]
        public MessageStatus Status { get; set; }

        /// <summary>
        /// The delivery time of the document
        /// </summary>
        [XmlElement("delivery-time")]
        public DateTime Deliverytime { get; set; }

        [XmlElement("primary-document")]
        public Document Primarydocument { get; set; }

        [XmlElement("attachment")]
        public List<Document> Attachment { get; set; }

        [XmlElement("link")]
        public List<Link> Link { get; set; }

        public override string ToString()
        {
            var attachments = Attachment.Aggregate(" ", (current, doc) => current + (doc.ToString() ));
            return
                string.Format(
                    "Deliverymethod: {0}, Status: {1}, Deliverytime: {2}, Primarydocument: {3}, " +
                    "Attachment: {4}", Deliverymethod, Status, Deliverytime, Primarydocument,
                    attachments);
        }
    }
}