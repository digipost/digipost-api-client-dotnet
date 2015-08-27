namespace Digipost.Api.Client.Domain.Print
{
    public interface INorwegianAddress : IAddress
    {
        string PostalCode { get; set; }
        
        string City { get; set; }
    }
}