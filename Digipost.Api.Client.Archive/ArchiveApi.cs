using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Api.Client.Archive.Actions;
using Digipost.Api.Client.Common.Entrypoint;
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

        Archive ArchiveDocuments(Archive archiveWithDocuments);

        Task<Archive> ArchiveDocumentsAsync(Archive archiveWithDocuments);

        /**
         * This will hash and create a Guid the same way as java UUID.nameUUIDFromBytes
         */
        Task<Stream> StreamDocumentFromExternalId(String externalId);

        Task<Stream> StreamDocumentFromExternalId(Guid externalIdGuid);
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
            var absoluteUri = _root.FindByRelationName("GET_ARCHIVES").AbsoluteUri();
            var archives = await _requestHelper.Get<Archives>(absoluteUri).ConfigureAwait(false);

            return archives.Archive.Select(ArchiveDataTransferObjectConverter.FromDataTransferObject);
        }

        public Archive ArchiveDocuments(Archive archiveWithDocuments)
        {
            var result = ArchiveDocumentsAsync(archiveWithDocuments);

            if (result.IsFaulted && result.Exception != null)
                throw result.Exception.InnerException;

            return result.Result;
        }

        public async Task<Archive> ArchiveDocumentsAsync(Archive archiveWithDocuments)
        {
            _logger.LogDebug($"Outgoing archive '{archiveWithDocuments.ArchiveDocuments.Count}' documents to archive: {archiveWithDocuments.Name ?? "default"}");

            var archiveUri = _root.FindByRelationName("ARCHIVE_DOCUMENTS").AbsoluteUri();

            var archiveAction = new ArchiveAction(archiveWithDocuments);
            var httpContent = archiveAction.Content(archiveWithDocuments);

            var task = _requestHelper.Post<V8.Archive>(httpContent, archiveAction.RequestContent, archiveUri);

            if (task.IsFaulted && task.Exception != null)
                throw task.Exception?.InnerException;

            var result = ArchiveDataTransferObjectConverter.FromDataTransferObject(await task.ConfigureAwait(false));

            _logger.LogDebug($"Response received for archiving to '{archiveWithDocuments.Name ?? "default"}'");

            return result;
        }

        public Task<Stream> StreamDocumentFromExternalId(String externalId)
        {
            var nameUuidFromBytes = UUIDInterop.NameUUIDFromBytes(externalId);
            return StreamDocumentFromExternalId(Guid.Parse(nameUuidFromBytes));
        }

        public async Task<Stream> StreamDocumentFromExternalId(Guid guid)
        {
            var uri = _root.FindByRelationName("GET_ARCHIVE_DOCUMENT_BY_UUID").Uri;

            var documentNyUuid = new Uri($"{uri}/{guid.ToString()}", UriKind.Absolute);
            var archive = await _requestHelper.Get<V8.Archive>(documentNyUuid).ConfigureAwait(false);
            var first = archive.Documents[0].Link.First(link => link.Rel.ToUpper().EndsWith("GET_ARCHIVE_DOCUMENT_CONTENT_STREAM"));

            return await _requestHelper.GetStream(new Uri(first.Uri, UriKind.Absolute)).ConfigureAwait(false);
        }
    }
}
