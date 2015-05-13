using System;
using System.ComponentModel;
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

        [XmlAttribute("media-type")]
        public string Mediatype { get; set; }
    }
}