using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Message : IMessage
    {
        public object SenderValue { get; set; }

        public SenderChoiceType SenderType { get; set; }
        
        public Recipient Recipient { get; set; }
        
        public DateTime? DeliveryTime { get; set; }
        
        public bool DeliveryTimeSpecified { get; private set; }
        
        public Document PrimaryDocument { get; set; }
        
        public List<Document> Attachments { get; set; }
    }
}
