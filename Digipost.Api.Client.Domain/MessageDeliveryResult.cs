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
        [XmlElement("message-id")]
        public string Messageid { get; set; }

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
                    "Messageid: {0}, Deliverymethod: {1}, Status: {2}, Deliverytime: {3}, Primarydocument: {4}, " +
                    "Attachment: {5}", Messageid, Deliverymethod, Status, Deliverytime, Primarydocument,
                    attachments);
        }
    }
}