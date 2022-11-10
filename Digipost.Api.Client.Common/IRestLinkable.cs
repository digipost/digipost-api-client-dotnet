using System.Collections.Generic;
using Digipost.Api.Client.Common.Entrypoint;

namespace Digipost.Api.Client.Common
{
    public abstract class RestLinkable
    {
        internal Dictionary<string, Link> Links { get; set; }
    }
}
