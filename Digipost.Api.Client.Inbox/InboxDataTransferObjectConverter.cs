using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;

namespace Digipost.Api.Client.Inbox
{
    internal class InboxDataTransferObjectConverter
    {
        internal static IEnumerable<InboxDocument> FromDataTransferObject(V8.Inbox inbox)
        {
            return inbox.Document?.Select(FromDataTransferObject) ?? new List<InboxDocument>();
        }

        internal static InboxDocument FromDataTransferObject(V8.Inbox_Document inboxdocument)
        {
            return new InboxDocument
            {
                Attachments = inboxdocument.Attachment?.Select(FromDataTransferObject) ?? new List<InboxDocument>(),
                AuthenticationLevel = inboxdocument.Authentication_Level.ToAuthenticationLevel(),
                Content = new Uri(inboxdocument.Content_Uri),
                ContentType = inboxdocument.Content_Type,
                Delete = new Uri(inboxdocument.Delete_Uri),
                DeliveryTime = inboxdocument.Delivery_Time,
                FirstAccessed = inboxdocument.First_AccessedSpecified ? inboxdocument.First_Accessed : (DateTime?) null,
                Id = inboxdocument.Id,
                Sender = inboxdocument.Sender,
                Subject = inboxdocument.Subject,
                ReferenceFromSender = inboxdocument.Reference_From_Sender
            };
        }
    }
}
