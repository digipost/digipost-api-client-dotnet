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
   public class SearchDetailsResult : ISearchDetailsResult
    {
        public List<SearchDetails> PersonDetails { get; set; }

        public override string ToString()
        {
            return $"PersonDetails: {string.Join(",", PersonDetails.Select(p => p.ToString()))}";
        }
    }
}