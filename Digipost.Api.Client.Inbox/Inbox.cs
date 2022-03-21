using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Inbox
{
    public class Inbox : IInbox
    {
        private readonly string _inboxRoot;
        private readonly RequestHelper _requestHelper;

        internal Inbox(Sender sender, RequestHelper requestHelper)
        {
            Sender = sender;
            _inboxRoot = $"{Sender.Id}/inbox";
            _requestHelper = requestHelper;
        }

        internal RequestHelper RequestHelper { get; set; }

        public Sender Sender { get; set; }

        public async Task<IEnumerable<InboxDocument>> Fetch(int offset = 0, int limit = 100)
        {
            var inboxPath = new Uri($"{_inboxRoot}?offset={offset}&limit={limit}", UriKind.Relative);

            var result = await _requestHelper.Get<V8.Inbox>(inboxPath).ConfigureAwait(false);

            return InboxDataTransferObjectConverter.FromDataTransferObject(result);
        }

        public async Task<Stream> FetchDocument(InboxDocument document)
        {
            var documentDataUri = new Uri($"{_inboxRoot}/{document.Id}/content", UriKind.Relative);

            return await _requestHelper.GetStream(documentDataUri).ConfigureAwait(false);
        }

        public async Task DeleteDocument(InboxDocument document)
        {
            await _requestHelper.Delete(document.Delete).ConfigureAwait(false);
        }
    }
}
