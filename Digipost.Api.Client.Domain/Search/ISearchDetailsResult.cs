using System.Collections.Generic;

namespace Digipost.Api.Client.Domain.Search
{
    public interface ISearchDetailsResult
    {
        IEnumerable<SearchDetails> PersonDetails { get; set; }
    }
}