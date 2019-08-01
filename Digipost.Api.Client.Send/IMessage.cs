using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Print;

namespace Digipost.Api.Client.Send
{
    public interface IMessage : IRequestContent
    {
        /// <summary>
        ///     An id that can be used for internal identification of a message. The id will exist in the returned
        ///     <see cref="MessageDeliveryResult" />.
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
        ///     The sender of the message, i.e. what the receiver of the message sees as the sender of the message.
        ///     If you are delivering a message on behalf of an organization with id 5555, set this property
        ///     to 5555. If you are delivering on behalf of yourself, set this to your organization`s sender id.
        ///     The id is created by Digipost.
        /// </summary>
        Sender Sender { get; set; }

        /// <summary>
        ///     Specifies fallback to print for the current message. If the message is not sent by Digipost,
        ///     then fallback to print will start.
        /// </summary>
        IPrintDetails PrintDetails { get; set; }
        
        /// <summary>
        ///     Specifies print-if-unread-after deadline for the current message. If the message is not read within the
        ///     deadline, it is sent to print with the contained printdetails.
        /// </summary>
        IPrintIfUnread PrintIfUnread { get; set; }
        
        /// <summary>
        ///     True if print-if-unread-after is specified for the message. Otherwise false.
        /// </summary>
        bool PrintIfUnreadAfterSpecified { get; }
    }
}
