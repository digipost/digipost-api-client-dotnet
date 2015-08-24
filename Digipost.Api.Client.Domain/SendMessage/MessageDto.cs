using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Message : IMessage
    {
        private Message()
        {
            /**Must exist for serialization.**/
        }

       public Message(Recipient recipient, Document primaryDocument)
        {
            Recipient = recipient;
            PrimaryDocument = primaryDocument;
            Attachments = new List<Document>();
        }

       public Message(Recipient recipient, Document primaryDocument, long senderId):
            this(recipient, primaryDocument)
        {
            SenderValue = senderId;
            SenderType = SenderChoiceType.SenderId; 
        }

        public Message(Recipient recipient, Document primaryDocument, SenderOrganization senderOrganization)
            : this(recipient, primaryDocument)
        {
            SenderValue = senderOrganization;
            SenderType = SenderChoiceType.SenderOrganization;
        }
        
        [XmlChoiceIdentifier("SenderType")]
        [XmlElement("sender-id", typeof(long))]
        [XmlElement("sender-organization", typeof(SenderOrganization))]
        public object SenderValue { get; set; }

        [XmlIgnore]
        public SenderChoiceType SenderType { get; set; }

       
        [XmlElement("recipient")]
        public Recipient Recipient { get; set; }

        [XmlElement("delivery-time")]
        public DateTime? DeliveryTime { get; set; }
        
        [XmlIgnoreAttribute()]
        public bool DeliveryTimeSpecified
        {
            get
            {
                return DeliveryTime != null;
            }
        }
        
        [XmlElement("primary-document")]
        public Document PrimaryDocument { get; set; }

        [XmlElement("attachment")]
        public List<Document> Attachments { get; set; }
    }
}