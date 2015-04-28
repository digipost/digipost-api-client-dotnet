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
    public enum Deliverymethod
    {
        /// <summary>
        ///     Delivered through fysical print and postal service.
        /// </summary>
        [XmlEnum("PRINT")]
        Print,

        /// <summary>
        ///     Delivered digitally in Digipost
        /// </summary>
        [XmlEnum("DIGIPOST")]
        Digipost
    }

    [Serializable]
    [XmlType(TypeName = "message-status", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-status", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum Messagestatus
    {
        /// <summary>
        ///     The message resource is not complete. Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("NOT_COMPLETE")]
        NotComplete,


        /// <summary>
        ///     The message resource is complete, and can be sent. Note that you can also tweak the message before sending it.
        ///     Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("COMPLETE")]
        Complete,


        /// <summary>
        /// The message is delivered. Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("DELIVERED")]
        Delivered,

        /// <summary>
        /// The message is delivered to print. Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("DELIVERED_TO_PRINT")]
        DeliveredToPrint
    }

   
}