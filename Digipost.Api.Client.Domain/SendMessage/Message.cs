using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Message : IMessage
    {
        private object _senderValue;

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
        /// message on behalf of an organization, and permission to do so is set, this is the parameter to set. </param>
        public Message(Recipient recipient, Document primaryDocument, long senderId) :
            this(recipient, primaryDocument)
        {
            SenderValue = senderId;
            SenderType = SenderChoiceType.SenderId;
        }

        public object SenderValue
        {
            get { return _senderValue; }
            set
            {
                _senderValue = value; 
                SenderType = SenderChoiceType.SenderId;
            }
        }

        public SenderChoiceType SenderType { get; set; }
        
        public Recipient Recipient { get; set; }
        
        public DateTime? DeliveryTime { get; set; }
        
        public bool DeliveryTimeSpecified { get; private set; }
        
        public Document PrimaryDocument { get; set; }
        
        public List<Document> Attachments { get; set; }
    }
}
