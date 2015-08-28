namespace Digipost.Api.Client.Domain.Search
{
    public interface ISearchDetails
    {
        string FirstName { get; set; }

        string MiddleName { get; set; }

        string LastName { get; set; }

        string DigipostAddress { get; set; }

        string MobileNumber { get; set; }

        string OrganizationName { get; set; }

        SearchDetailsAddress SearchDetailsAddress { get; set; }
    }
}