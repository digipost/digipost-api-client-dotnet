namespace Digipost.Api.Client.Common.Search
{
    public class SearchDetailsAddress : ISearchDetailsAddress
    {
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string HouseLetter { get; set; }

        public string AdditionalAddressLine { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public override string ToString()
        {
            return $"Street: {Street}, HouseNumber: {HouseNumber}, HouseLetter: {HouseLetter}, AdditionalAddressLine: {AdditionalAddressLine}, PostalCode: {PostalCode}, City: {City}";
        }
    }
}