using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Api.Client.Archive.Actions;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Utilities;
using Microsoft.Extensions.Logging;
using V8;

namespace Digipost.Api.Client.Archive
{
    internal interface IArchiveApi
    {
        /// <summary>
        /// List all the archives available to the current sender
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Archive>> FetchArchives();

        Task<Archive> ArchiveDocuments(Archive archiveWithDocuments);

        Task<Archive> ArchiveDocumentsAsync(Archive archiveWithDocuments);

        Task<IEnumerable<Archive>> FetchArchiveDocumentsByReferenceId(string referenceId);

        Task<ArchiveDocument> FetchArchiveDocument(GetArchiveDocumentByUuidUri getArchiveDocumentByUuidUri);

        Task<Archive> FetchArchiveDocuments(ArchiveNextDocumentsUri nextDocumentsUri);

        Task<ArchiveDocument> UpdateDocument(ArchiveDocument archiveDocument, ArchiveDocumentUpdateUri updateUri);

        Task DeleteDocument(ArchiveDocumentDeleteUri deleteUri);

        /**
         * This will hash and create a Guid the same way as java UUID.nameUUIDFromBytes
         */
        Task<Archive> GetArchiveDocument(GetArchiveDocumentByUuidUri getArchiveDocumentUri);

        Task<Archive> FetchDocumentFromExternalId(String externalId);

        Task<Archive> FetchDocumentFromExternalId(Guid externalIdGuid);

        Task<Stream> StreamDocumentFromExternalId(String externalId);

        Task<Stream> StreamDocumentFromExternalId(Guid externalIdGuid);

        Task<Stream> StreamDocument(ArchiveDocumentContentStreamUri documentContentStreamUri);

        Task<ArchiveDocumentContent> GetDocumentContent(ArchiveDocumentContentUri archiveDocumentContentUri);
    }

    public class ArchiveApi : IArchiveApi
    {
        private readonly Root _root;
        private readonly RequestHelper _requestHelper;
        private readonly ILogger<ArchiveApi> _logger;

        internal ArchiveApi(RequestHelper requestHelper, ILoggerFactory loggerFactory, Root root)
        {
            _root = root;
            _logger = loggerFactory.CreateLogger<ArchiveApi>();
            _requestHelper = requestHelper;
        }

        public async Task<IEnumerable<Archive>> FetchArchives()
        {
            var archivesUri = _root.GetGetArchivesUri();
            var archives = await _requestHelper.Get<Archives>(archivesUri).ConfigureAwait(false);

            return archives.Archive.Select(ArchiveDataTransferObjectConverter.FromDataTransferObject);
        }

        public async Task<IEnumerable<Archive>> FetchArchiveDocumentsByReferenceId(string referenceId)
        {
            var archives = await _requestHelper.Get<Archives>(_root.GetGetArchiveDocumentsReferenceIdUri(referenceId)).ConfigureAwait(false);

            return archives.Archive.Select(ArchiveDataTransferObjectConverter.FromDataTransferObject);
        }

        public async Task<ArchiveDocument> FetchArchiveDocument(GetArchiveDocumentByUuidUri nextDocumentsUri)
        {
            var archive = await GetArchiveDocument(nextDocumentsUri).ConfigureAwait(false);
            return archive.One();
        }

        public async Task<Archive> FetchArchiveDocuments(ArchiveNextDocumentsUri nextDocumentsUri)
        {
            var result = await _requestHelper.Get<V8.Archive>(nextDocumentsUri).ConfigureAwait(false);

            return ArchiveDataTransferObjectConverter.FromDataTransferObject(result);
        }

        public async Task<Archive> GetArchiveDocument(GetArchiveDocumentByUuidUri getArchiveDocumentUri)
        {
            var result = await _requestHelper.Get<V8.Archive>(getArchiveDocumentUri).ConfigureAwait(false);
            return ArchiveDataTransferObjectConverter.FromDataTransferObject(result);
        }

        public async Task<ArchiveDocument> UpdateDocument(ArchiveDocument archiveDocument, ArchiveDocumentUpdateUri updateUri)
        {
            var messageAction = new ArchiveDocumentAction(archiveDocument);
            var httpContent = messageAction.Content(archiveDocument);

            var updatedArchiveDocument = _requestHelper.Put<Archive_Document>(httpContent, messageAction.RequestContent, updateUri);

            if (updatedArchiveDocument.IsFaulted && updatedArchiveDocument.Exception != null)
                throw updatedArchiveDocument.Exception?.InnerException;

            return ArchiveDataTransferObjectConverter.FromDataTransferObject(await updatedArchiveDocument.ConfigureAwait(false));
        }

        public async Task DeleteDocument(ArchiveDocumentDeleteUri deleteUri)
        {
            await _requestHelper.Delete(deleteUri);
        }

        public Task<Archive> ArchiveDocuments(Archive archiveWithDocuments)
        {
            var result = ArchiveDocumentsAsync(archiveWithDocuments);

            if (result.IsFaulted && result.Exception != null)
                throw result.Exception.InnerException;

            return result;
        }

        public async Task<Archive> ArchiveDocumentsAsync(Archive archiveWithDocuments)
        {
            _logger.LogDebug("Outgoing archive '{count}' documents to archive: {name}", archiveWithDocuments.ArchiveDocuments.Count, archiveWithDocuments.Name ?? "default");

            var archiveUri = _root.GetArchiveDocumentsUri();

            var archiveAction = new ArchiveAction(archiveWithDocuments);
            var httpContent = archiveAction.Content(archiveWithDocuments);

            var task = _requestHelper.Post<V8.Archive>(httpContent, archiveAction.RequestContent, archiveUri);

            if (task.IsFaulted && task.Exception != null)
                throw task.Exception?.InnerException;

            var result = ArchiveDataTransferObjectConverter.FromDataTransferObject(await task.ConfigureAwait(false));

            _logger.LogDebug("Response received for archiving to '{name}'", archiveWithDocuments.Name ?? "default");

            return result;
        }

        public async Task<Archive> FetchDocumentFromExternalId(string externalId)
        {
            var result = await _requestHelper.Get<V8.Archive>(_root.GetGetArchiveDocumentsByUuidUri(externalId)).ConfigureAwait(false);
            return ArchiveDataTransferObjectConverter.FromDataTransferObject(result);
        }

        public async Task<Archive> FetchDocumentFromExternalId(Guid externalIdGuid)
        {
            var result = await _requestHelper.Get<V8.Archive>(_root.GetGetArchiveDocumentsByUuidUri(externalIdGuid)).ConfigureAwait(false);
            return ArchiveDataTransferObjectConverter.FromDataTransferObject(result);
        }

        public async Task<Stream> StreamDocumentFromExternalId(string externalId)
        {
            var archive = GetArchiveDocument(_root.GetGetArchiveDocumentsByUuidUri(externalId)).Result;
            var documentContentStreamUri = archive.One().GetDocumentContentStreamUri();

            return await StreamDocument(documentContentStreamUri);
        }

        public async Task<Stream> StreamDocumentFromExternalId(Guid guid)
        {
            var archive = GetArchiveDocument(_root.GetGetArchiveDocumentsByUuidUri(guid)).Result;
            var documentContentStreamUri = archive.One().GetDocumentContentStreamUri();

            return await StreamDocument(documentContentStreamUri);
        }

        public async Task<Stream> StreamDocument(ArchiveDocumentContentStreamUri documentContentStreamUri)
        {
            return await _requestHelper.GetStream(documentContentStreamUri).ConfigureAwait(false);
        }

        public async Task<ArchiveDocumentContent> GetDocumentContent(ArchiveDocumentContentUri archiveDocumentContentUri)
        {
            var result = await _requestHelper.Get<Archive_Document_Content>(archiveDocumentContentUri).ConfigureAwait(false);

            return ArchiveDataTransferObjectConverter.FromDataTransferObject(result);
        }
    }
}
