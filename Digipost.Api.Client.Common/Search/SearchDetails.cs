using System.Collections.Generic;

namespace Digipost.Api.Client.Common.Search
{
    /// <summary>
    ///     When requesting person information via the SearchDetails API, this class is the result.
    /// </summary>
    public class SearchDetails : ISearchDetails
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string DigipostAddress { get; set; }

        public string MobileNumber { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationNumber { get; set; }

        public IEnumerable<SearchDetailsAddress> SearchDetailsAddress { get; set; }

        public override string ToString()
        {
            return $"FirstName: {FirstName}, MiddleName: {MiddleName}, LastName: {LastName}, DigipostAddress: {DigipostAddress}, MobileNumber: {MobileNumber}, OrganizationName: {OrganizationName}, SearchDetailsAddress: {SearchDetailsAddress}";
        }
    }
}