using System;
using System.Linq;
using Digipost.Api.Client.Domain.Mailbox;
using Digipost.Api.Client.Domain.Extensions;

namespace Digipost.Api.Client.Mailbox
{
    internal class DataTransferObjectConverter
    {
        internal static Inbox FromDataTransferObject(inbox inbox)
        {
            return new Inbox
            {
                Documents = inbox.document.Select(FromDataTransferObject)
            };
        }

        internal static InboxDocument FromDataTransferObject(inboxdocument inboxdocument)
        {
            return new InboxDocument
            {
                Attachments = inboxdocument.attachment.Select(FromDataTransferObject),
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
