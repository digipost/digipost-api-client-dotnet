using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Message : IMessage
    {
        public IDigipostRecipient DigipostRecipient { get; set; }

        /// <summary>
        /// A message to be delivered to a Recipient. 
        /// </summary>
        /// <param name="digipostRecipient">The recipient recieving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        public Message(IDigipostRecipient digipostRecipient, IDocument primaryDocument)
        {
            DigipostRecipient = digipostRecipient;
            PrimaryDocument = primaryDocument;
            Attachments = new List<IDocument>();
        }

        public IPrintRecipient PrintRecipient { get; set; }

        /// <summary>
        /// The id of the sender, created by Digipost.  If you are delivering a 
        /// message on behalf of an organization, and permission to do so is set, this is the parameter to set.
        /// </summary>
        public string SenderId { get; set; }
        
        public DateTime? DeliveryTime { get; set; }

        public bool DeliveryTimeSpecified
        {
            get { return DeliveryTime != null; }
        }

        public IDocument PrimaryDocument { get; set; }
        
        public List<IDocument> Attachments { get; set; }
    }
}
