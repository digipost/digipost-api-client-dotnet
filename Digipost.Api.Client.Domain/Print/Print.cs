namespace Digipost.Api.Client.Domain.Print
{
    public abstract class Print : IPrintAddress
    {
        protected Print(string name, Address address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; set; }

        public Address Address { get; set; }
    }
}
