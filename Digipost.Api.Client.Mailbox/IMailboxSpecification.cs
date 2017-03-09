using System.IO;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain.Mailbox;

namespace Digipost.Api.Client.Mailbox
{
    interface IMailboxSpecification
    {
        Task<Inbox> FetchInbox(int offset = 0, int limit = 100);

        Task<Stream> FetchDocument(InboxDocument document);

        Task DeleteDocument(InboxDocument document);
    }
}
