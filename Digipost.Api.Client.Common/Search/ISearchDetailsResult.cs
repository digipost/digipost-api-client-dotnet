using System.Collections.Generic;

namespace Digipost.Api.Client.Common.Search
{
    public interface ISearchDetailsResult
    {
        IEnumerable<SearchDetails> PersonDetails { get; set; }
    }
}