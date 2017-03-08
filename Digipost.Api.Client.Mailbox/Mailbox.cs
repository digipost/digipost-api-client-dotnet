using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Domain.Mailbox;

namespace Digipost.Api.Client.Mailbox
{
    public class Mailbox : IMailboxSpecification
    {
        private readonly RequestHelper _requestHelper;
        private readonly string _inboxRoot;

        public Mailbox(string senderId, RequestHelper requestHelper)
        {
            SenderId = senderId;
            _inboxRoot = $"{SenderId}/inbox";
            _requestHelper = requestHelper;
        }

        public string SenderId { get; set; }
    
        public async Task<Inbox> FetchInbox(int offset = 0, int limit = 100)
        {
            var inboxPath = new Uri($"{_inboxRoot}?offset={offset}&limit={limit}", UriKind.Relative);
            
            var result = await _requestHelper.GenericGetAsync<inbox>(inboxPath).ConfigureAwait(false);
            
            return DataTransferObjectConverter.FromDataTransferObject(result);
        }

        public async Task<Stream> FetchDocument(InboxDocument document)
        {
            var documentDataUri = new Uri($"{_inboxRoot}/{document.Id}/content", UriKind.Relative);

            return await _requestHelper.GetStream(documentDataUri);
        }

        public Stream DeleteDocument(InboxDocument document)
        {
            throw new NotImplementedException();
        }
    }
}