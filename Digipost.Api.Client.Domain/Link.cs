using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    public class Link
    {
        public Link(string rel, string uri, string mediaType)
        {
            Rel = rel;
            UriString = uri;
            MediaType = mediaType;
        }

        public string Rel { get; set; }

        public string UriString { get; set; }

        public Uri Uri => new Uri(Uri.EscapeUriString(UriString));

        public string MediaType { get; set; }

        public string LocalPath => Uri.LocalPath.Substring(1);

        public string Mediatype { get; set; }

        public override string ToString()
        {
            return string.Format("Rel: {0}, UriString: {1}, Uri: {2}, MediaType: {3}, LocalPath: {4}, Mediatype: {5}", Rel, UriString, Uri, MediaType, LocalPath, Mediatype);
        }
    }
}