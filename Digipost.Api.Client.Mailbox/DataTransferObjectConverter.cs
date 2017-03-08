using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.Extensions;
using Digipost.Api.Client.Domain.Mailbox;

namespace Digipost.Api.Client.Mailbox
{
    internal class DataTransferObjectConverter
    {
        internal static Inbox FromDataTransferObject(inbox inbox)
        {
            return new Inbox
            {
                Documents = inbox.document?.Select(FromDataTransferObject)?? new List<InboxDocument>()
            };
        }

        internal static InboxDocument FromDataTransferObject(inboxdocument inboxdocument)
        {
            return new InboxDocument
            {
                Attachments = inboxdocument.attachment?.Select(FromDataTransferObject) ?? new List<InboxDocument>(),
                AuthenticationLevel = inboxdocument.authenticationlevel.ToAuthenticationLevel(),
                Content = new Uri(inboxdocument.contenturi),
                ContentType = inboxdocument.contenttype,
                Delete = new Uri(inboxdocument.deleteuri),
                DeliveryTime = inboxdocument.deliverytime,
                FirstAccessed = inboxdocument.firstaccessedSpecified ? inboxdocument.firstaccessed : (DateTime?) null,
                Id = inboxdocument.id,
                Sender = inboxdocument.sender,
                Subject = inboxdocument.subject
            };
        }
    }
}