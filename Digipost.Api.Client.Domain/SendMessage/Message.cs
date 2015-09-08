using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Message : IMessage
    {
        /// <summary>
        /// A message to be delivered to a Recipient. 
        /// </summary>
        /// <param name="recipient">The recipient recieving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        /// <param name="senderId">The id of the sender, created by Digipost.  If you are delivering a 
        /// message on behalf of an organization, and permission to do so is set, this is the parameter to set. </param>
        public Message(IRecipient recipient, IDocument primaryDocument, string senderId = null)
        {
            Recipient = recipient;
            PrimaryDocument = primaryDocument;
            SenderId = senderId;
            Attachments = new List<IDocument>();
        }

        public string SenderId { get; set; }

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
