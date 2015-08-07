using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "link", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("link", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class Link
    {
        [XmlAttribute("rel")]
        public string Rel { get; set; }

        [XmlAttribute("uri")]
        public string Uri { get; set; }

        public string MediaType { get; set; }
        
        [XmlIgnore]
        public string LocalPath
        {
            get
            {
                return new Uri(Uri).LocalPath;
            }
        }

        [XmlAttribute("media-type")]
        public string Mediatype { get; set; }

        public Link(string rel, string uri, string mediaType)
        {
            Rel = rel;
            Uri = uri;
            MediaType = mediaType;
        }

        private Link()
        {
            /* Exists for serialization */    
        }
    }
}