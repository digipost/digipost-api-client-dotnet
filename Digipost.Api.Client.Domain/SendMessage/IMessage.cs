using System;
using System.Collections.Generic;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IMessage : IRequestContent
    {
        /// <summary>
        /// The recipient receiving the message. 
        /// </summary>
        Recipient Recipient { get; set; }

        /// <summary>
        /// Optional. The time when the document will be made visible to the user. 
        /// </summary>
        DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// True if a delivery time is specified for the message. Otherwise false.
        /// </summary>
        bool DeliveryTimeSpecified { get; }

        /// <summary>
        /// The primary document of the delivery. This is the document that will be shown first in the 
        /// recipient's inbox when opening the letter.
        /// </summary>
        IDocument PrimaryDocument { get; set; }

        /// <summary>
        /// Optional. Attachments can be added to the message, and can be of same types as the primary
        /// document.
        /// </summary>
        List<IDocument> Attachments { get; set; }
        
    }
}