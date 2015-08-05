using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Autocomplete
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlTypeAttribute("autocomplete", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class AutocompleteResult
    {

        [XmlElement("suggestion")]
        public List<Suggestion> Suggestion { get; set; }
    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlTypeAttribute("suggestion", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Suggestion
    {
        [XmlElement("search-string")]
        public string SearchString { get; set; }

        [XmlElement("link")]
        public Link Result { get; set; }

    }
}
