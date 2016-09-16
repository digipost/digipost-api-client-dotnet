using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IMessage : IRequestContent
    {
        /// <summary>
        ///   The actual sender of the message. This is used in scenarios where one party, the broker, is creating a message
        ///   on behalf of another, the sender. It is only possible if the sender has granted the broker the right to send
        ///   on its behalf.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     The recipient receiving the message.
        /// </summary>
        IDigipostRecipient DigipostRecipient { get; set; }

        /// <summary>
        ///     Optional. The delivery time of the message as it appears in the receiver's inbox.
        ///     The use of this field is limited to those with explicit permission. If set in the future, the 
        ///     message will not appear until the set time. Historical delivery time is not allowed.
        /// </summary>

        DateTime? DeliveryTime { get; set; }

        /// <summary>
        ///     True if a delivery time is specified for the message. Otherwise false.
        /// </summary>
        bool DeliveryTimeSpecified { get; }

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

        /// <summary>
        ///     The id of the sender, created by Digipost.  If you are delivering a
        ///     message on behalf of an organization, and permission to do so is set, this is the parameter to set.
        /// </summary>
        string SenderId { get; set; }

        /// <summary>
        ///     Specifies fallback to print for the current message. If the message is not sent by Digipost,
        ///     then fallback to print will start.
        /// </summary>
        IPrintDetails PrintDetails { get; set; }
    }
}