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
        /// The type of postage delivery method.
        /// </summary>
        [XmlElement("delivery-method")]
        public DeliveryMethod Deliverymethod { get; set; }

        /// <summary>
        /// The current status of the message in Digipost.
        /// </summary>
        [XmlElement("status")]
        public MessageStatus Status { get; set; }

        /// <summary>
        /// Optional. The time when the document will be made visible to the user. 
        /// </summary>
        [Obsolete("Use DeliveryTime insted. This is just a rename of field name and has no side effects. NB! This will be removed in future verion.")]
        public DateTime Deliverytime { get { return DeliveryTime; } set { DeliveryTime = value; } }

        /// <summary>
        /// Optional. The time when the document will be made visible to the user. 
        /// </summary>
        [XmlElement("delivery-time")]
        public DateTime DeliveryTime { get; set; }

        /// <summary>
        /// The primary document of the delivery. This is the document that will be shown first in the 
        /// recipient's inbox when opening the letter.
        /// </summary>
        [Obsolete("Use PrimaryDocument insted. This is just a rename of field name and has no side effects. NB! This will be removed in future version.")]
        public Document Primarydocument { get { return PrimaryDocument; } set { PrimaryDocument = value; } }

        /// <summary>
        /// The primary document of the delivery. This is the document that will be shown first in the 
        /// recipient's inbox when opening the letter.
        /// </summary>
        [XmlElement("primary-document")]
        public Document PrimaryDocument { get; set; }
        
        /// <summary>
        /// Optional. Attachments can be added to the message, and can be of same types as the primary
        /// document.
        /// </summary>
        [XmlElement("attachment")]
        public List<Document> Attachment { get; set; }

        [XmlElement("link")]
        [Obsolete("This field has no relevant information and will therefore be removed in future version.")]
        public List<Link> Link { get; set; }

        public override string ToString()
        {
            var attachments = Attachment.Aggregate(" ", (current, doc) => current + (doc.ToString() ));
            return
                string.Format(
                    "Deliverymethod: {0}, Status: {1}, DeliveryTime: {2}, Primarydocument: {3}, " +
                    "Attachment: {4}", Deliverymethod, Status, Deliverytime, Primarydocument,
                    attachments);
        }
    }
}