using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Message : RequestContent
    {
        private Message()
        {
            /**Must exist for serialization.**/
        }

        /// <summary>
        /// A message to be delivered to a Recipient. 
        /// </summary>
        /// <param name="recipient">The recipient recieving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        public Message(Recipient recipient, Document primaryDocument)
        {
            Recipient = recipient;
            PrimaryDocument = primaryDocument;
            Attachments = new List<Document>();
        }

        /// <summary>
        /// A message to be delivered to a Recipient. 
        /// </summary>
        /// <param name="recipient">The recipient receiving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        /// <param name="senderId">The id of the sender, created by Digipost.  If you are delivering a 
        /// message on behalf of an organization, and permission to do so is set, this is the parameter use. </param>
        public Message(Recipient recipient, Document primaryDocument, string senderId):
            this(recipient, primaryDocument)
        {
            SenderValue = senderId;
            SenderType = SenderChoiceType.SenderId; 
        }

        /// <summary>
        /// A message to be delivered to a Recipient. 
        /// </summary>
        /// <param name="recipient">The recipient receiving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        /// <param name="senderOrganization">The organization sending the message. If you are delivering a 
        /// message on behalf of an organization, and permission to do so is set, this is the parameter to set.
        /// </param>
        public Message(Recipient recipient, Document primaryDocument, SenderOrganization senderOrganization)
            : this(recipient, primaryDocument)
        {
            SenderValue = senderOrganization;
            SenderType = SenderChoiceType.SenderOrganization;
        }
        
        [XmlChoiceIdentifier("SenderType")]
        [XmlElement("sender-id", typeof(string))]
        [XmlElement("sender-organization", typeof(SenderOrganization))]
        public object SenderValue { get; set; }

        [XmlIgnore]
        public SenderChoiceType SenderType { get; set; }

        /// <summary>
        /// The recipient receiving the message. 
        /// </summary>
        [XmlElement("recipient")]
        public Recipient Recipient { get; set; }

        /// <summary>
        /// Optional. The time when the document will be made visible to the user. 
        /// </summary>
        [XmlElement("delivery-time")]
        public DateTime? DeliveryTime { get; set; }
        
        /// <summary>
        /// True if a delivery time is specified for the message. Otherwise false.
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool DeliveryTimeSpecified
        {
            get
            {
                return DeliveryTime != null;
            }
        }
        
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
        public List<Document> Attachments { get; set; }
    }
}