using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Print
{
    public class ForeignAddress : Address, IForeignAddress
    {
        /// <summary>
        ///     Foreign address for use when sending a letter abroad.
        /// </summary>
        /// <param name="countryIdentifier">Type of identifier used for identifying a country.</param>
        /// <param name="countryIdentifierValue">The value for country.</param>
        /// <param name="addressLine1">First address line.</param>
        /// <param name="addressLine2">Second address line.</param>
        /// <param name="addressLine3">Third address line.</param>
        /// <param name="addressLine4">Fourth address line.</param>
        public ForeignAddress(CountryIdentifier countryIdentifier, string countryIdentifierValue,
            string addressLine1, string addressLine2 = null, string addressLine3 = null, string addressLine4 = null)
            : base(addressLine1, addressLine2, addressLine3)
        {
            AddressLine4 = addressLine4;
            CountryIdentifier = countryIdentifier;
            CountryIdentifierValue = countryIdentifierValue;
        }

        public string AddressLine4 { get; set; }

        public string CountryIdentifierValue { get; set; }

        public CountryIdentifier CountryIdentifier { get; set; }

        public override string ToString()
        {
            return $"Country: {CountryIdentifier.Country}, AddressLine1: {AddressLine1}, AddressLine2: {AddressLine2}, AddressLine3: {AddressLine3}, AddressLine4: {AddressLine4}";
        }
    }
}
