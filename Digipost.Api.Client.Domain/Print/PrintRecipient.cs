namespace Digipost.Api.Client.Domain.Print
{
    
    public class PrintRecipient : PrintAddress
    {
        /// <summary>
        ///     Constructor for foreign(not Norwegian) recipients
        /// </summary>
        public PrintRecipient(string name, ForeignAddress address)
        {
            Name = name;
            Address = address;
        }

        /// <summary>
        ///     Constructor for Norwegian recipients
        /// </summary>
        public PrintRecipient(string name, NorwegianAddress address)
        {
            Name = name;
            Address = address;
        }

        private PrintRecipient()
        {
            /**must exist for serializing**/
        }
    }
}