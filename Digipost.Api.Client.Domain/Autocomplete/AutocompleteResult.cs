using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Autocomplete
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("autocomplete", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]   
    public class AutocompleteResult
    {
        [XmlElement("suggestion")]
        public List<Suggestion> Suggestion { get; set; }
    }
}
