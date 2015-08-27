namespace Digipost.Api.Client.Domain.Print
{
    public class PrintReturnAddress : PrintAddress
    {
        /// <summary>
        ///     Constructor for foreign(not Norwegian) return address
        /// </summary>
        public PrintReturnAddress(string name, ForeignAddressDataTransferObject addressDataTransferObject)
        {
            Name = name;
            Address = addressDataTransferObject;
        }

        /// <summary>
        ///     Constructor for Norwegian return address
        /// </summary>
        public PrintReturnAddress(string name, NorwegianAddressDataTransferObject addressDataTransferObject)
        {
            Name = name;
            Address = addressDataTransferObject;
        }

        private PrintReturnAddress()
        {
            /**must exist for serializing**/
        }
    }
}