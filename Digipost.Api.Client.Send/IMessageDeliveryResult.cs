using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Send
{
    public interface IMessageDeliveryResult
    {
        /// <summary>
        ///     The id of the message that produced this delivery result.
        /// </summary>
        string MessageId { get; set; }

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
        DateTime? DeliveryTime { get; set; }

        /// <summary>
        ///     The primary document of the delivery. This is the document that will be shown first in the
        ///     recipient's inbox when opening the letter.
        /// </summary>
        IDocument PrimaryDocument { get; set; }

        /// <summary>
        ///     Optional. Attachments can be added to the message, and can be of same types as the primary
        ///     document.
        /// </summary>
        IEnumerable<IDocument> Attachments { get; set; }
    }
}
