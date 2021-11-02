using System;
using System.IO;
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

        public async Task<Stream> FetchDocument(String externalId)
        {
            var guid = new Guid(externalId);

            var documentDataUri = new Uri($"{_archiveRoot}/{guid.ToString()}/contentstream", UriKind.Relative);

            return await _requestHelper.GetStream(documentDataUri).ConfigureAwait(false);
        }

    }
}
