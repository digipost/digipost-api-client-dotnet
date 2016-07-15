using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IMessageDeliveryResult
    {
        /// <summary>
        ///     The type of postage delivery method.
        /// </summary>
        DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        ///     The current status of the message in Digipost.
        /// </summary>
        MessageStatus Status { get; set; }

        /// <summary>
        ///     Optional. The time when the document will be made visible to the user.
        /// </summary>
        DateTime DeliveryTime { get; set; }

        /// <summary>
        ///     The primary document of the delivery. This is the document that will be shown first in the
        ///     recipient's inbox when opening the letter.
        /// </summary>
        IDocument PrimaryDocument { get; set; }

        /// <summary>
        ///     Optional. Attachments can be added to the message, and can be of same types as the primary
        ///     document.
        /// </summary>
        List<IDocument> Attachments { get; set; }
    }
}