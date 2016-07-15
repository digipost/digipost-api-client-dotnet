namespace Digipost.Api.Client.Domain.Print
{
    public class PrintReturnRecipient : Print, IPrintReturnRecipient
    {
        public PrintReturnRecipient(string name, Address address)
            : base(name, address)
        {
        }
    }
}