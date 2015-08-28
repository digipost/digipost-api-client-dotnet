using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    public class MessageDeliveryResult : IMessageDeliveryResult
    {
        public DeliveryMethod Deliverymethod { get; set; }
        
        public MessageStatus Status { get; set; }
        
        public DateTime Deliverytime { get; set; }
        
        public DateTime DeliveryTime { get; set; }
        
        public DocumentDataTransferObject Primarydocument { get; set; }
        
        public DocumentDataTransferObject PrimaryDocumentDataTransferObject { get; set; }
        
        public List<DocumentDataTransferObject> Attachment { get; set; }
        
        public List<Link> Link { get; set; }
    }
}
