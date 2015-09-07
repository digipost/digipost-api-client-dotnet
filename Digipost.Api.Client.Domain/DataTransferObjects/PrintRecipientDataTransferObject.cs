namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    public class PrintRecipientDataTransferObject : PrintDataTransferObject
    {
        public PrintRecipientDataTransferObject(string name, ForeignAddressDataTransferObject addressDataTransferObject)
        {
            Name = name;
            Address = addressDataTransferObject;
        }

        public PrintRecipientDataTransferObject(string name, NorwegianAddressDataTransferObject addressDataTransferObject)
        {
            Name = name;
            Address = addressDataTransferObject;
        }

        private PrintRecipientDataTransferObject()
        {
            /**must exist for serializing**/
        }
    }
}