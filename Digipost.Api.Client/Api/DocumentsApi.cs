using System;
using System.Threading.Tasks;
using Digipost.Api.Client.Common.Entrypoint;
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
    }

    internal class DocumentsApi : IDocumentsApi
    {
        private readonly RequestHelper _requestHelper;
        private readonly ILoggerFactory _loggerFactory;
        private readonly Root _root;

        public DocumentsApi(RequestHelper requestHelper, ILoggerFactory loggerFactory, Root root)
        {
            _requestHelper = requestHelper;
            _loggerFactory = loggerFactory;
            _root = root;
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
    }
}
