using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Extensions;
using V8;

namespace Digipost.Api.Client.Inbox
{
    internal static class InboxDataTransferObjectConverter
    {
        internal static IEnumerable<InboxDocument> FromDataTransferObject(this V8.Inbox inbox)
        {
            return inbox.Document?.Select(FromDataTransferObject) ?? new List<InboxDocument>();
        }

        internal static InboxDocument FromDataTransferObject(this Inbox_Document inboxDocument)
        {
            return new InboxDocument
            {
                Attachments = inboxDocument.Attachment?.Select(FromDataTransferObject) ?? new List<InboxDocument>(),
                AuthenticationLevel = inboxDocument.Authentication_Level.ToAuthenticationLevel(),
                ContentType = inboxDocument.Content_Type,
                DeliveryTime = inboxDocument.Delivery_Time,
                FirstAccessed = inboxDocument.First_AccessedSpecified ? inboxDocument.First_Accessed : (DateTime?) null,
                Id = inboxDocument.Id,
                Sender = inboxDocument.Sender,
                Subject = inboxDocument.Subject,
                ReferenceFromSender = inboxDocument.Reference_From_Sender,
                Links = inboxDocument.Link.FromDataTransferObject()
            };
        }
    }
}
