using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Autocomplete
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("suggestion", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class AutocompleteSuggestion
    {
        /// <summary>
        /// When searching for a person via the Autocomplete API, an AutocompleteSuggestion
        /// is a representation of a concrete result. Note that this result does not contain
        /// person information. To get the personal info, use this class as input to the 
        /// Person Details API. 
        /// </summary>
        /// <param name="searchString">The actual search string result.</param>
        /// <param name="link">The link to the personal info.</param>
        public AutocompleteSuggestion(string searchString, Link link)
        {
            SearchString = searchString;
            Link = link;
        }

        private AutocompleteSuggestion()
        {
            /* Exists for serialization */    
        }

        [XmlElement("search-string")]
        public string SearchString { get; set; }

        [XmlElement("link")]
        public Link Link { get; set; }

        public override string ToString()
        {
            return string.Format("SearchString: {0}, Link: {1}", SearchString, Link);
        }
    }
}
