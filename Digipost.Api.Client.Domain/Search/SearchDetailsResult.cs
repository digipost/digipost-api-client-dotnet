using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Search
{
    /// <summary>
    ///     A collection of Person Details as a result of a request via the Person Details API.
    /// </summary>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("recipients", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6",IsNullable = false)]
    public class SearchDetailsResult : ISearchDetailsResult
    {
        [XmlElement("recipient")]
        public List<SearchDetails> PersonDetails { get; set; }

        public override string ToString()
        {
            return $"PersonDetails: {string.Join(",", PersonDetails.Select(p => p.ToString()))}";
        }
    }
}