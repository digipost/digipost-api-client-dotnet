using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Utilities;
using V8 = Digipost.Api.Client.Common.Generated.V8;

namespace Digipost.Api.Client.Inbox
{
    internal interface IInbox
    {
        Task<IEnumerable<InboxDocument>> Fetch(int offset = 0, int limit = 100);

        Task<Stream> FetchDocument(GetInboxDocumentContentUri document);

        Task DeleteDocument(InboxDocumentDeleteUri document);
    }

    public class Inbox : IInbox
    {
        private readonly Root _inboxRoot;
        private readonly RequestHelper _requestHelper;

        internal Inbox(RequestHelper requestHelper, Root root)
        {
            _inboxRoot = root;
            _requestHelper = requestHelper;
        }

        public async Task<IEnumerable<InboxDocument>> Fetch(int offset = 0, int limit = 100)
        {
            var inboxPath = _inboxRoot.GetGetInboxUri(offset, limit);

            var result = await _requestHelper.Get<V8.Inbox>(inboxPath).ConfigureAwait(false);

            return result.FromDataTransferObject();
        }

        public async Task<Stream> FetchDocument(GetInboxDocumentContentUri getInboxDocumentContentUri)
        {
            return await _requestHelper.GetStream(getInboxDocumentContentUri).ConfigureAwait(false);
        }

        public async Task DeleteDocument(InboxDocumentDeleteUri deleteUri)
        {
            await _requestHelper.Delete(deleteUri).ConfigureAwait(false);
        }
    }
}
