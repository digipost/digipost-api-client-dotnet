using System;
using System.IO;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Mailbox;

namespace Digipost.Api.Client.Mailbox
{
    public class Mailbox : IMailboxSpecification
    {
        public Mailbox(string senderId, RequestHelper requestHelper)
        {
            SenderId = senderId;
        }

        public string SenderId { get; set; }

        public Inbox FetchInbox(int offset = 0, int limit = 100)
        {
            throw new NotImplementedException();
        }

        public Stream FetchDocument(InboxDocument document)
        {
            throw new NotImplementedException();
        }

        public Stream DeleteDocument(InboxDocument document)
        {
            throw new NotImplementedException();
        }
    }
}