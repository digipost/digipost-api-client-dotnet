using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Extensions;
using V8 = Digipost.Api.Client.Common.Generated.V8;

namespace Digipost.Api.Client.Inbox
{
    internal static class InboxDataTransferObjectConverter
    {
        internal static IEnumerable<InboxDocument> FromDataTransferObject(this V8.Inbox inbox)
        {
            return inbox.Document?.Select(FromDataTransferObject) ?? new List<InboxDocument>();
        }

        internal static InboxDocument FromDataTransferObject(this V8.InboxDocument inboxDocument)
        {
            return new InboxDocument
            {
                Attachments = inboxDocument.Attachment?.Select(FromDataTransferObject) ?? new List<InboxDocument>(),
                AuthenticationLevel = inboxDocument.AuthenticationLevel.ToAuthenticationLevel(),
                ContentType = inboxDocument.ContentType,
                DeliveryTime = inboxDocument.DeliveryTime,
                FirstAccessed = inboxDocument.FirstAccessedSpecified ? inboxDocument.FirstAccessed : (DateTime?) null,
                Id = inboxDocument.Id,
                Sender = inboxDocument.Sender,
                Subject = inboxDocument.Subject,
                ReferenceFromSender = inboxDocument.ReferenceFromSender,
                Links = inboxDocument.Link.FromDataTransferObject()
            };
        }
    }
}
