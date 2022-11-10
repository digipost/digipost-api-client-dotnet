using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Extensions;
using V8;

namespace Digipost.Api.Client.Inbox
{
    internal class InboxDataTransferObjectConverter
    {
        internal static IEnumerable<InboxDocument> FromDataTransferObject(V8.Inbox inbox)
        {
            return inbox.Document?.Select(FromDataTransferObject) ?? new List<InboxDocument>();
        }

        internal static InboxDocument FromDataTransferObject(Inbox_Document inboxdocument)
        {
            return new InboxDocument
            {
                Attachments = inboxdocument.Attachment?.Select(FromDataTransferObject) ?? new List<InboxDocument>(),
                AuthenticationLevel = inboxdocument.Authentication_Level.ToAuthenticationLevel(),
                ContentType = inboxdocument.Content_Type,
                DeliveryTime = inboxdocument.Delivery_Time,
                FirstAccessed = inboxdocument.First_AccessedSpecified ? inboxdocument.First_Accessed : (DateTime?) null,
                Id = inboxdocument.Id,
                Sender = inboxdocument.Sender,
                Subject = inboxdocument.Subject,
                ReferenceFromSender = inboxdocument.Reference_From_Sender,
                Links = DataTransferObjectConverter.FromDataTransferObject(inboxdocument.Link)
            };
        }
    }
}
