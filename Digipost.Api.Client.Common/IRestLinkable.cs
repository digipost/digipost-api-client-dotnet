using System.Collections.Generic;
using Digipost.Api.Client.Common.Entrypoint;

namespace Digipost.Api.Client.Common
{
    public abstract class RestLinkable
    {
        protected RestLinkable()
        {
        }

        protected RestLinkable(Dictionary<string, Link> links)
        {
            Links = links;
        }

        internal Dictionary<string, Link> Links { get; set; }
    }
}
