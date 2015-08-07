using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Autocomplete
{
    /// <summary>
    /// A collection of autocomplete suggestions based on a search. See AutocompleteSuggestion
    /// for more information.
    /// </summary>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("autocomplete", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]   
    public class AutocompleteSuggestionResults
    {
        [XmlElement("suggestion")]
        public List<AutocompleteSuggestion> AutcompleteSuggestions { get; set; }
    }
}
