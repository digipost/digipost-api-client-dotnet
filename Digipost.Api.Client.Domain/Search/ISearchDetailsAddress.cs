namespace Digipost.Api.Client.Domain.Search
{
    public interface ISearchDetailsAddress
    {
        string Street { get; set; }

        string HouseNumber { get; set; }
        
        string HouseLetter { get; set; }
        
        string AdditionalAddressLine { get; set; }
        
        string ZipCode { get; set; }
        
        string City { get; set; }
    }
}