using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Utilities;
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

        internal ArchiveApi(RequestHelper requestHelper, Root root)
        {
            _root = root;
            _requestHelper = requestHelper;
        }

        public async Task<IEnumerable<Archive>> FetchArchives()
        {
            var absoluteUri = _root.FindByRelationName("GET_ARCHIVES").AbsoluteUri();
            var archives = await _requestHelper.Get<Archives>(absoluteUri).ConfigureAwait(false);

            return archives.Archive.Select(ArchiveDataTransferObjectConverter.FromDataTransferObject);
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
