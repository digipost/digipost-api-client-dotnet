namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    public class PrintReturnRecipientDataTransferObject : PrintDataTransferObject
    {
        /// <summary>
        ///     Constructor for foreign(not Norwegian) return address
        /// </summary>
        public PrintReturnRecipientDataTransferObject(string name, ForeignAddressDataTransferObject foreignAddressDataTransferObject)
            : base(name, foreignAddressDataTransferObject)
        {
        }

        /// <summary>
        ///     Constructor for Norwegian return address
        /// </summary>
        public PrintReturnRecipientDataTransferObject(string name, NorwegianAddressDataTransferObject norwegianAddressDataTransferObject)
            : base(name, norwegianAddressDataTransferObject)
        {
        }

        private PrintReturnRecipientDataTransferObject()
        {
            /**must exist for serializing**/
        }
    }
}