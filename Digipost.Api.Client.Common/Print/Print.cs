namespace Digipost.Api.Client.Common.Print
{
    public abstract class Print : IPrint
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