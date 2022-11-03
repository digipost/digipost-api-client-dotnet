using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Archive
{
    public class Archive : IArchive
    {
        private readonly Root _root;
        private readonly RequestHelper _requestHelper;

        internal Archive(RequestHelper requestHelper, Root root)
        {
            _root = root;
            _requestHelper = requestHelper;
        }
        public Task<Stream> StreamDocumentFromExternalId(String externalId)
        {
            var nameUuidFromBytes = UUIDInterop.NameUUIDFromBytes(externalId);
            return StreamDocumentFromExternalId(Guid.Parse(nameUuidFromBytes));
        }

        public async Task<Stream> StreamDocumentFromExternalId(Guid guid)
        {
            var uri  =_root.FindByRelationName("GET_ARCHIVE_DOCUMENT_BY_UUID").Uri;

            var documentNyUuid = new Uri($"{uri}/{guid.ToString()}", UriKind.Relative);
            var archive = await _requestHelper.Get<V8.Archive>(documentNyUuid).ConfigureAwait(false);
            var first = archive.Documents[0].Link.First(link => link.Rel.ToUpper().EndsWith("GET_ARCHIVE_DOCUMENT_CONTENT_STREAM"));

            return await _requestHelper.GetStream(new Uri(first.Uri, UriKind.Absolute)).ConfigureAwait(false);
        }
    }
}
