namespace Digipost.Api.Client.Domain.Print
{
    public class PrintReturnAddress : PrintAddress
    {
          /// <summary>
        ///     Constructor for foreign(not Norwegian) return address
        /// </summary>
        public PrintReturnAddress(string name, ForeignAddress address)
        {
            Name = name;
            Address = address;
        }

        /// <summary>
        ///     Constructor for Norwegian return address
        /// </summary>
        public PrintReturnAddress(string name, NorwegianAddress address)
        {
            Name = name;
            Address = address;
        }

        private PrintReturnAddress()
        {
            /**must exist for serializing**/
        }
    }
}
