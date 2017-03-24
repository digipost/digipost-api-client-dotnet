namespace Digipost.Api.Client.Common.Print
{
    public interface IPrintReturnRecipient
    {
        string Name { get; set; }

        Address Address { get; set; }
    }
}