using System.Collections.Generic;

namespace Digipost.Api.Client.Common.Search
{
    public interface ISearchDetails
    {
        string FirstName { get; set; }

        string MiddleName { get; set; }

        string LastName { get; set; }

        string DigipostAddress { get; set; }

        string MobileNumber { get; set; }

        string OrganizationName { get; set; }

        string OrganizationNumber { get; set; }

        IEnumerable<SearchDetailsAddress> SearchDetailsAddress { get; set; }
    }
}