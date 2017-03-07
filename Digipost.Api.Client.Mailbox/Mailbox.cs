using System;
using System.IO;
using System.Threading.Tasks;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Mailbox;

namespace Digipost.Api.Client.Mailbox
{
    public class Mailbox : IMailboxSpecification
    {
        private readonly RequestHelper _requestHelper;

        public Mailbox(string senderId, RequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
            SenderId = senderId;
        }

        public string SenderId { get; set; }

        public async Task<Inbox> FetchInbox(int offset = 0, int limit = 100)
        {
            var inboxPath = new Uri($"{SenderId}/inbox?offset={offset}&maxResults=100", UriKind.Relative);

            var result =_requestHelper.GenericGetAsync<inbox>(inboxPath);

            return DataTransferObjectConverter.FromDataTransferObject(await result);
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