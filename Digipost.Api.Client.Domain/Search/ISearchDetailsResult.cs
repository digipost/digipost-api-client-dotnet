using System.Collections.Generic;

namespace Digipost.Api.Client.Domain.Search
{
    public interface ISearchDetailsResult
    {
        List<SearchDetails> PersonDetails { get; set; }
    }
}