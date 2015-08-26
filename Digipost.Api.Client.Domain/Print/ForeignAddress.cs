using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Print
{
    public class ForeignAddress : IForeignAddress
    {
        /// <summary>
        /// Foreign address for use when sending a letter abroad.
        /// </summary>
        /// <param name="countryIdentifier">Type of identifier used for identifying a country.</param>
        /// <param name="countryIdentifierValue">The value for country.</param>
        /// <param name="addressline1">First address line.</param>
        /// <param name="addressline2">Second address line.</param>
        /// <param name="addressline3">Third address line.</param>
        /// <param name="addressline4">Fourth address line.</param>
        public ForeignAddress(CountryIdentifier countryIdentifier, string countryIdentifierValue,
            string addressline1, string addressline2 = null, string addressline3 = null, string addressline4 = null)
        {
            Addressline1 = addressline1;
            Addressline2 = addressline2;
            Addressline3 = addressline3;
            Addressline4 = addressline4;
            CountryIdentifier = countryIdentifier;
            CountryIdentifierValue = countryIdentifierValue;
        }

        public string Addressline1 { get; set; }

        public string Addressline2 { get; set; }

        public string Addressline3 { get; set; }

        public string Addressline4 { get; set; }

        /// <summary>
        /// Identifier value for country.
        /// </summary>
        public string CountryIdentifierValue { get; set; }

        /// <summary>
        /// Type of identifier used for identifying a country.
        /// </summary>
        public CountryIdentifier CountryIdentifier { get; set; }
    }
}
