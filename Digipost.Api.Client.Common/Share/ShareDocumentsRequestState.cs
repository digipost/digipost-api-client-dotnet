using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Relations;

namespace Digipost.Api.Client.Common.Share
{
    public class ShareDocumentsRequestState : RestLinkable
    {
        public DateTime? SharedAtTime { get; }

        public DateTime? ExpiryTime { get; }

        public DateTime? WithdrawnTime { get; }

        public IEnumerable<SharedDocument> SharedDocuments { get; }

        public ShareDocumentsRequestState(DateTime? sharedAtTime,
            DateTime? expiryTime,
            DateTime? withdrawnTime,
            IEnumerable<SharedDocument> sharedDocuments, Dictionary<string, Link> links)
        {
            SharedAtTime = sharedAtTime;
            ExpiryTime = expiryTime;
            WithdrawnTime = withdrawnTime;
            SharedDocuments = sharedDocuments;
            Links = links;
        }

        public AddAdditionalDataUri GetStopSharingUri()
        {
            return new AddAdditionalDataUri(Links["STOP_SHARING"]);
        }
    }
}
