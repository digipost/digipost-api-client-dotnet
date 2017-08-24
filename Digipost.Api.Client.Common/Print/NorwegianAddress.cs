namespace Digipost.Api.Client.Common.Print
{
    public class NorwegianAddress : Address, INorwegianAddress
    {
        /// <summary>
        ///     Norwegian address for use when sending a letter within Norway.
        /// </summary>
        /// <param name="postalCode">Postal code for the address provided.</param>
        /// <param name="city">City in which the address resides.</param>
        /// <param name="addressLine1">First address line.</param>
        /// <param name="addressLine2">Second address line. Optional.</param>
        /// <param name="addressLine3">Third address line. Optional. </param>
        public NorwegianAddress(string postalCode,
            string city, string addressLine1, string addressLine2 = null, string addressLine3 = null)
            : base(addressLine1, addressLine2, addressLine3)
        {
            City = city;
            PostalCode = postalCode;
        }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}
