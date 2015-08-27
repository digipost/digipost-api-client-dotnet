namespace Digipost.Api.Client.Domain.Print
{
    public class NorwegianAddress : INorwegianAddress
    {
        /// <summary>
        /// Norwegian address for use when sending a letter within Norway.
        /// </summary>
        /// <param name="postalCode">Postal code for the address provided.</param>
        /// <param name="city">City in which the address resides.</param>
        /// <param name="addressline1">First address line.</param>
        /// <param name="addressline2">Second address line.</param>
        /// <param name="addressline3">Third address line.</param>
        public NorwegianAddress(string postalCode,
            string city, string addressline1, string addressline2 = null, string addressline3 = null)
        {
            Addressline1 = addressline1;
            Addressline2 = addressline2;
            Addressline3 = addressline3;
            City = city;
            PostalCode = postalCode;
        }

        public string Addressline1 { get; set; }

        public string Addressline2 { get; set; }
        
        public string Addressline3 { get; set; }
        
        public string PostalCode { get; set; }
        
        public string City { get; set; }
    }
}
