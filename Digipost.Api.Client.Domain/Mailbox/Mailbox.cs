using System;
using System.IO;

namespace Digipost.Api.Client.Domain.Mailbox
{
    public class Mailbox : IMailboxSpecification
    {
        public Mailbox(string senderId)
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