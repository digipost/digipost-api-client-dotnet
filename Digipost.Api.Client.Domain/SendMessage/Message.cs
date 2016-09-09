using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Message : IMessage
    {
        /// <summary>
        ///     A message to be delivered to a Recipient.
        /// </summary>
        /// <param name="digipostRecipient">The recipient recieving the message.</param>
        /// <param name="primaryDocument">The primary document sent to the recipient.</param>
        public Message(IDigipostRecipient digipostRecipient, IDocument primaryDocument)
        {
            DigipostRecipient = digipostRecipient;
            PrimaryDocument = primaryDocument;
            Attachments = new List<IDocument>();
        }

        public string Id { get; set; }

        public IDigipostRecipient DigipostRecipient { get; set; }

        public IPrintDetails PrintDetails { get; set; }

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