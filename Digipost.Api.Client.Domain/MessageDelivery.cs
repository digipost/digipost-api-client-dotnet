using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-delivery", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-delivery", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class MessageDelivery
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
    } 
}