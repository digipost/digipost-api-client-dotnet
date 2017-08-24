namespace Digipost.Api.Client.Common.Print
{
    public interface IPrint
    {
        /// <summary>
        ///     Sets the name of the recipient.(Also used for the return address)
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Sets the address of the recipient.(Also used for the return address) Choose between ForeignAddress or
        ///     NorwegianAddress.
        /// </summary>
        Address Address { get; set; }
    }
}
