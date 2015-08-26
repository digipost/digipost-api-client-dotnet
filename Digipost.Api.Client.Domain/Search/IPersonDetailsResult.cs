using System.Collections.Generic;

namespace Digipost.Api.Client.Domain.PersonDetails
{
    public interface IPersonDetailsResult
    {
        List<PersonDetails> PersonDetails { get; set; }
    }
}