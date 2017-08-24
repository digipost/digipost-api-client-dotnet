namespace Digipost.Api.Client.Common.Print
{
    public interface INorwegianAddress : IAddress
    {
        string PostalCode { get; set; }

        string City { get; set; }
    }
}
