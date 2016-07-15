namespace Digipost.Api.Client.Domain.Print
{
    public interface IPrintRecipient
    {
        string Name { get; set; }

        Address Address { get; set; }
    }
}