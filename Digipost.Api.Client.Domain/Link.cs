using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "link", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("link", Namespace = "http://api.digipost.no/schema/v6",IsNullable = true)]
    public class Link
    {
        public Link(string rel, string uri, string mediaType)
        {
            Rel = rel;
            UriString = uri;
            MediaType = mediaType;
        }

        private Link()
        {
            /* Exists for serialization */
        }

        [XmlAttribute("rel")]
        public string Rel { get; set; }

        [XmlAttribute("uri")]
        public string UriString { get; set; }

        [XmlIgnore]
        public Uri Uri
        {
            get { return new Uri(Uri.EscapeUriString(UriString)); }
        }

        public string MediaType { get; set; }

        [XmlIgnore]
        public string LocalPath
        {
            get { return Uri.LocalPath.Substring(1); }
        }

        [XmlAttribute("media-type")]
        public string Mediatype { get; set; }

        public override string ToString()
        {
            return string.Format("Rel: {0}, UriString: {1}, Uri: {2}, MediaType: {3}, LocalPath: {4}, Mediatype: {5}", Rel, UriString, Uri, MediaType, LocalPath, Mediatype);
        }
    }
}