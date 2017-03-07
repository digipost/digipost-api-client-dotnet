using System.Collections.Generic;
using Digipost.Api.Client.Domain.Mailbox;

namespace Digipost.Api.Client.Mailbox
{
    public class Inbox
    {
        public IEnumerable<InboxDocument> Documents { get; set; }
    }
}