using System.IO;

namespace Digipost.Api.Client.Domain.Mailbox
{
    interface IMailboxSpecification
    {
        Inbox FetchInbox(int offset = 0, int limit = 100);

        Stream FetchDocument(InboxDocument document);

        Stream DeleteDocument(InboxDocument document);
    }
}
