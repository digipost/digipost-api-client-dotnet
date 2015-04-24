using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-delivery", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-delivery", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Messagedelivery
    {
        [XmlElement("message-id")]
        public string Messageid { get; set; }

        [XmlElement("delivery-method")]
        public Deliverymethod Deliverymethod { get; set; }

        [XmlElement("status")]
        public Messagestatus Status { get; set; }

        [XmlElement("delivery-time")]
        public DateTime Deliverytime { get; set; }

        [XmlElement("primary-document")]
        public Document Primarydocument { get; set; }

        [XmlElement("attachment")]
        public List<Document> Attachment { get; set; }

        [XmlElement("link")]
        public List<Link> Link { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "delivery-method", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("delivery-method", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    internal enum Deliverymethod
    {
        /// <remarks />
        PRINT,

        /// <remarks />
        DIGIPOST
    }

    [Serializable]
    [XmlType(TypeName = "message-status", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-status", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    internal enum Messagestatus
    {
        /// <remarks />
        NOT_COMPLETE,

        /// <remarks />
        COMPLETE,

        /// <remarks />
        DELIVERED,

        /// <remarks />
        DELIVERED_TO_PRINT
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "link", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("link", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    internal class Link
    {
        [XmlAttribute("rel")]
        public string Rel { get; set; }

        [XmlAttribute("uri")]
        public string Uri { get; set; }

        [XmlAttribute("media-type")]
        public string Mediatype { get; set; }
    }
}