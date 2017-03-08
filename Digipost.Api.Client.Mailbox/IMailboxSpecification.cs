using System.IO;
using System.Threading.Tasks;
using Digipost.Api.Client.Mailbox;

namespace Digipost.Api.Client.Domain.Mailbox
{
    interface IMailboxSpecification
    {
        Task<Inbox> FetchInbox(int offset = 0, int limit = 100);

        Task<Stream> FetchDocument(InboxDocument document);

        Stream DeleteDocument(InboxDocument document);
    }
}
