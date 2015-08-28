using System.Collections.Generic;

namespace Digipost.Api.Client.Domain.PersonDetails
{
    public interface IPersonDetailsResult
    {
        List<SearchDetails> PersonDetails { get; set; }
    }
}