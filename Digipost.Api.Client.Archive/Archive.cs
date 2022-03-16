using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Utilities;


namespace Digipost.Api.Client.Archive
{
    public class Archive : IArchive
    {
        private readonly string _archiveRoot;
        private readonly RequestHelper _requestHelper;

        internal Archive(Sender sender, RequestHelper requestHelper)
        {
            Sender = sender;
            _archiveRoot = $"{Sender.Id}/archive";
            _requestHelper = requestHelper;
        }

        internal RequestHelper RequestHelper { get; set; }

        public Sender Sender { get; set; }

        public Task<Stream> StreamDocumentFromExternalId(String externalId)
        {
            var nameUuidFromBytes = UUIDInterop.NameUUIDFromBytes(externalId);
            return StreamDocumentFromExternalId(Guid.Parse(nameUuidFromBytes));
        }

        public async Task<Stream> StreamDocumentFromExternalId(Guid guid)
        {
            var documentNyUuid = new Uri($"{_archiveRoot}/document/uuid/{guid.ToString()}", UriKind.Relative);
            var archive = await _requestHelper.Get<V8.Archive>(documentNyUuid).ConfigureAwait(false);
            var first = archive.Documents[0].Link.First(link => link.Rel.EndsWith("get_archive_document_content_stream"));

            return await _requestHelper.GetStream(new Uri(first.Uri, UriKind.Absolute)).ConfigureAwait(false);
        }

    }
}
