namespace Digipost.Api.Client.Domain.Print
{
    public interface INorwegianAddress
    {
        string Addressline1 { get; set; }

        string Addressline2 { get; set; }
        
        string Addressline3 { get; set; }
        
        string PostalCode { get; set; }
        
        string City { get; set; }
    }
}