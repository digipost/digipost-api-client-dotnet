using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Inbox
{
    internal interface IInbox
    {
        Task<IEnumerable<InboxDocument>> FetchInbox(int offset = 0, int limit = 100);

        Task<Stream> FetchDocument(InboxDocument document);

        Task DeleteDocument(InboxDocument document);
    }
}
