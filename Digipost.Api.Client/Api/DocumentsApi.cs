using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Share;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Send;
using Microsoft.Extensions.Logging;
using V8;

namespace Digipost.Api.Client.Api
{
    public interface IDocumentsApi
    {
        /**
         * Guid should be the Guid added to the actual document, usually the main document
         */
        Task<DocumentStatus> GetDocumentStatus(Guid guid);
        Task<DocumentStatus> GetDocumentStatusAsync(Guid guid);

        Task<DocumentEvents> GetDocumentEvents(DateTime from, DateTime to, int offset, int maxResults);
        Task<DocumentEvents> GetDocumentEventsAsync(DateTime from, DateTime to, int offset, int maxResults);
    }
    public interface IShareDocumentsApi
    {
        Task<ShareDocumentsRequestState> GetShareDocumentsRequestState(Guid guid);
        Task<ShareDocumentsRequestState> GetShareDocumentsRequestStateAsync(Guid guid);
        Task<SharedDocumentContent> GetShareDocumentContent(GetSharedDocumentContentUri uri);
        Task<SharedDocumentContent> GetShareDocumentContentAsync(GetSharedDocumentContentUri uri);
        Task<Stream> FetchSharedDocument(GetSharedDocumentContentStreamUri uri);
    }

    internal class DocumentsApi : IDocumentsApi, IShareDocumentsApi
    {
        private readonly RequestHelper _requestHelper;
        private readonly ILoggerFactory _loggerFactory;
        private readonly Root _root;
        private readonly Sender _sender;

        public DocumentsApi(RequestHelper requestHelper, ILoggerFactory loggerFactory, Root root, Sender sender)
        {
            _requestHelper = requestHelper;
            _loggerFactory = loggerFactory;
            _root = root;
            _sender = sender;
        }

        public Task<DocumentStatus> GetDocumentStatus(Guid guid)
        {
            var result = GetDocumentStatusAsync(guid);

            if (result.IsFaulted && result.Exception != null)
                throw result.Exception.InnerException;

            return result;
        }

        public async Task<DocumentStatus> GetDocumentStatusAsync(Guid guid)
        {
            var documentStatusUri = _root.GetDocumentStatusUri(guid);
            var result = await _requestHelper.Get<Document_Status>(documentStatusUri).ConfigureAwait(false);

            return result.FromDataTransferObject();
        }

        public Task<DocumentEvents> GetDocumentEvents(DateTime from, DateTime to, int offset, int maxResults)
        {
            var result = GetDocumentEventsAsync(from, to, offset, maxResults);

            if (result.IsFaulted && result.Exception != null)
                throw result.Exception.InnerException;

            return result;
        }

        public async Task<DocumentEvents> GetDocumentEventsAsync(DateTime from, DateTime to, int offset, int maxResults)
        {
            var documentEventsUri = _root.GetDocumentEventsUri(_sender, from, to, offset, maxResults);
            var result = await _requestHelper.Get<Document_Events>(documentEventsUri).ConfigureAwait(false);

            return result.FromDataTransferObject();
        }

        public Task<ShareDocumentsRequestState> GetShareDocumentsRequestState(Guid guid)
        {
            var result = GetShareDocumentsRequestStateAsync(guid);

            if (result.IsFaulted && result.Exception != null)
                throw result.Exception.InnerException;

            return result;
        }

        public async Task<ShareDocumentsRequestState> GetShareDocumentsRequestStateAsync(Guid guid)
        {
            var shareDocumentsRequestStateUri = _root.GetShareDocumentsRequestStateUri(guid);
            var result = await _requestHelper.Get<Share_Documents_Request_State>(shareDocumentsRequestStateUri).ConfigureAwait(false);

            return result.FromDataTransferObject();
        }

        public Task<SharedDocumentContent> GetShareDocumentContent(GetSharedDocumentContentUri uri)
        {
            var result = GetShareDocumentContentAsync(uri);
            if (result.IsFaulted && result.Exception != null)
                throw result.Exception.InnerException;

            return result;
        }

        public async Task<SharedDocumentContent> GetShareDocumentContentAsync(GetSharedDocumentContentUri uri)
        {
            var result = await _requestHelper.Get<Shared_Document_Content>(uri).ConfigureAwait(false);

            return result.FromDataTransferObject();
        }

        public async Task<Stream> FetchSharedDocument(GetSharedDocumentContentStreamUri uri)
        {
            return await _requestHelper.GetStream(uri).ConfigureAwait(false);
        }
    }
}
