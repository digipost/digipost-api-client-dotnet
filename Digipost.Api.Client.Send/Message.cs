using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Print;

namespace Digipost.Api.Client.Send
{
    public class Message : IMessage
    {
        /// <summary>
        ///     A message to be delivered to a Recipient.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the message, i.e. what the receiver of the message sees as the sender of the
        ///     message.
        /// </param>
        /// <param name="digipostRecipient">
        ///     The recipient recieving the message.
        /// </param>
        /// <param name="primaryDocument">
        ///     The primary document sent to the recipient.
        /// </param>
        public Message(Sender sender, IDigipostRecipient digipostRecipient, IDocument primaryDocument)
        {
            Sender = sender;
            DigipostRecipient = digipostRecipient;
            PrimaryDocument = primaryDocument;
            Attachments = new List<IDocument>();
        }

        public string Id { get; set; }

        public IDigipostRecipient DigipostRecipient { get; set; }

        public IPrintDetails PrintDetails { get; set; }
        
        public IPrintIfUnread PrintIfUnread { get; set; }

        public bool PrintIfUnreadAfterSpecified => PrintIfUnread != null;

        public Sender Sender { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public bool DeliveryTimeSpecified => DeliveryTime != null;

        public IDocument PrimaryDocument { get; set; }

        public List<IDocument> Attachments { get; set; }
    }
}
