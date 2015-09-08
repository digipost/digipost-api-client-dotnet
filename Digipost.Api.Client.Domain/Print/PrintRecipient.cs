namespace Digipost.Api.Client.Domain.Print
{
    public class PrintRecipient : Print, IPrintRecipient
    {
        public PrintRecipient(string name, Address address) : base(name,address)
        {
        }
    }
}
