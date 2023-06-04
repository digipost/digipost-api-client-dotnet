namespace Digipost.Api.Client.Common.Print
{
    public class PrintRecipient : Print, IPrintRecipient
    {
        public PrintRecipient(string name, Address address)
            : base(name, address)
        {
        }

        public override string ToString()
        {
            return $"FullName: {Name}, Address: {Address}";
        }
    }
}
