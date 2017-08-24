using System.Collections.Generic;
using System.Linq;

namespace Digipost.Api.Client.Common.Search
{
    /// <summary>
    ///     A collection of Person Details as a result of a request via the Person Details API.
    /// </summary>
    public class SearchDetailsResult : ISearchDetailsResult
    {
        public IEnumerable<SearchDetails> PersonDetails { get; set; }

        public override string ToString()
        {
            return $"PersonDetails: {string.Join(",", PersonDetails.Select(p => p.ToString()))}";
        }
    }
}
