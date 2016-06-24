using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    public class MessageDeliveryResult : IMessageDeliveryResult
    {
        public DateTime Deliverytime { get; set; }

        public List<Link> Link { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime DeliveryTime { get; set; }

        public IDocument PrimaryDocument { get; set; }

        public List<IDocument> Attachments { get; set; }
    }
}