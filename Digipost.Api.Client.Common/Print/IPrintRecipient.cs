namespace Digipost.Api.Client.Common.Print
{
    public interface IPrintRecipient
    {
        string Name { get; set; }

        Address Address { get; set; }
    }
}