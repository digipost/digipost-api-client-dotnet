using System.Collections.Generic;

namespace Digipost.Api.Client.Domain.Mailbox
{
    internal class Inbox
    {
        public IEnumerable<InboxDocument> Documents { get; set; }
    }
}