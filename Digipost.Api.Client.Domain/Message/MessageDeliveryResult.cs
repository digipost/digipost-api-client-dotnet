using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-delivery", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-delivery", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class MessageDeliveryResult
    {

        [XmlElement("delivery-method")]
        public DeliveryMethod Deliverymethod { get; set; }

        [XmlElement("status")]
        public MessageStatus Status { get; set; }

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