using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Mailbox
{
    internal class InboxDocument
    {
        public long Id { get; set; }

        public string Subject { get; set; }

        public string Sender { get; set; }

        public DateTime DeliveryTime { get; set; }

        public DateTime FirstAccessed { get; set; }

        public AuthenticationLevel AuthenticationLevel { get; set; }

        public string ContentType { get; set; }

        public Uri Content { get; set; }

        public Uri Delete { get; set; }

        public IEnumerable<InboxDocument> Attachments { get; set; }
    }
}