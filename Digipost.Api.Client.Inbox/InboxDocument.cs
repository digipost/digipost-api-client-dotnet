using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Relations;

namespace Digipost.Api.Client.Inbox
{
    public class InboxDocument : RestLinkable
    {
        public long Id { get; set; }

        public string Subject { get; set; }

        public string Sender { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime? FirstAccessed { get; set; }

        public AuthenticationLevel AuthenticationLevel { get; set; }

        public string ContentType { get; set; }

        public IEnumerable<InboxDocument> Attachments { get; set; } = new List<InboxDocument>();

        public string ReferenceFromSender { get; set; }

        public GetInboxDocumentContentUri GetGetDocumentContentUri()
        {
            return new GetInboxDocumentContentUri(Links["GET_DOCUMENT_CONTENT"]);
        }
        public InboxDocumentDeleteUri GetDeleteUri()
        {
            return new InboxDocumentDeleteUri(Links["SELF_DELETE"]);
        }
    }
}
