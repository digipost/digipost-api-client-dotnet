using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Inbox
{
    public class Inbox : IInbox
    {
        private readonly string _inboxRoot;
        private readonly RequestHelper _requestHelper;

        internal Inbox(RequestHelper requestHelper, Root root)
        {
            _inboxRoot = root.FindByRelationName("GET_INBOX").Uri;
            _requestHelper = requestHelper;
        }

        public async Task<IEnumerable<InboxDocument>> Fetch(int offset = 0, int limit = 100)
        {
            var inboxPath = new Uri($"{_inboxRoot}?offset={offset}&limit={limit}", UriKind.Absolute);

            var result = await _requestHelper.Get<V8.Inbox>(inboxPath).ConfigureAwait(false);

            return InboxDataTransferObjectConverter.FromDataTransferObject(result);
        }

        public async Task<Stream> FetchDocument(InboxDocument document)
        {
            return await _requestHelper.GetStream(document.Content).ConfigureAwait(false);
        }

        public async Task DeleteDocument(InboxDocument document)
        {
            await _requestHelper.Delete(document.Delete).ConfigureAwait(false);
        }
    }
}
