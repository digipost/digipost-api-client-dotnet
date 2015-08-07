using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Autocomplete
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("suggestion", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Suggestion
    {
        public Suggestion(string searchString, Link link)
        {
            SearchString = searchString;
            Link = link;
        }

        private Suggestion()
        {
            /* Exists for serialization */    
        }

        [XmlElement("search-string")]
        public string SearchString { get; set; }

        [XmlElement("link")]
        public Link Link { get; set; }
    }
}
