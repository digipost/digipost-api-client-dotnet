namespace Digipost.Api.Client.Domain.Print
{
    public interface IPrintReturnRecipient
    {
        string Name { get; set; }

        Address Address { get; set; }
    }
}