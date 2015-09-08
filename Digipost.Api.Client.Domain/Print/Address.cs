namespace Digipost.Api.Client.Domain.Print
{
    public abstract class Address : IAddress
    {
        protected Address(string addressLine1 = null, string addressLine2 = null, string addressLine3 = null)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressLine3 = addressLine3;
        }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }
    }
}
