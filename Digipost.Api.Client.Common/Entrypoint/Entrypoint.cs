using System.Collections.Generic;
using System.Linq;

namespace Digipost.Api.Client.Common.Entrypoint
{
    /// <summary>
    /// The Root entrypoint is the starting point of the REST-api of Digipost.
    /// </summary>
    public class Root
    {
        public Root(string certificate, List<Link> links)
        {
            Certificate = certificate;
            Links = links;
        }

        /// <summary>
        /// The element certificate contains Digipost's current public key. It can be used to verify
        /// each response from Digipost. See the documentation for more information.
        /// </summary>
        public string Certificate { get; set; }

        /// <summary>
        /// List of actions that are available to the current broker
        /// </summary>
        public List<Link> Links { get; }

        /// <summary>
        /// Find the Link for a given relation name
        /// </summary>
        /// <param name="relationName">the relation name</param>
        /// <returns>the link</returns>
        public Link FindByRelationName(string relationName)
        {
            return Links.Find(l => l.Rel.ToUpper().EndsWith(relationName));
        }
    }

    public class Link
    {
        public Link(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// The actual uri for the resource
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// A string constant representing the Relation of the uri. The rel is Optional
        /// but is in practice always present.
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// An optional media type describing the resource
        /// </summary>
        public string MediaType { get; set; }

    }
}
