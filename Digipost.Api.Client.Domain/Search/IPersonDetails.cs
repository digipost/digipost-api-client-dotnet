namespace Digipost.Api.Client.Domain.PersonDetails
{
    public interface IPersonDetails
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string DigipostAddress { get; set; }
        string MobileNumber { get; set; }
        string OrganizationName { get; set; }
        PersonDetailsAddress PersonDetailsAddress { get; set; }
    }
}