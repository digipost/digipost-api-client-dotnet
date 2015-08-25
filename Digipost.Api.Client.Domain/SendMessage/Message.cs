using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Message : IMessage
    {
        private string _senderId;

        /// <summary>
        /// A message to be delivered to a Recipient. 
        /// </summary>
        /// <param name="recipient">The recipient recieving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        public Message(IRecipient recipient, IDocument primaryDocument)
        {
            Recipient = recipient;
            PrimaryDocument = primaryDocument;
            Attachments = new List<IDocument>();
        }

        /// <summary>
        /// A message to be delivered to a Recipient. 
        /// </summary>
        /// <param name="recipient">The recipient receiving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        /// <param name="senderId">The id of the sender, created by Digipost.  If you are delivering a 
        /// message on behalf of an organization, and permission to do so is set, this is the parameter to set. </param>
        public Message(IRecipient recipient, IDocument primaryDocument, string senderId) :
            this(recipient, primaryDocument)
        {
            SenderId = senderId;
            SenderType = SenderChoiceType.SenderId;
        }

        public string SenderId
        {
            get { return _senderId; }
            set
            {
                _senderId = value; 
                SenderType = SenderChoiceType.SenderId;
            }
        }

        internal SenderChoiceType SenderType { get; set; }
        
        public IRecipient Recipient { get; set; }
        
        public DateTime? DeliveryTime { get; set; }

        public bool DeliveryTimeSpecified
        {
            get { return DeliveryTime != null; }
        }

        public IDocument PrimaryDocument { get; set; }
        
        public List<IDocument> Attachments { get; set; }
    }
}
