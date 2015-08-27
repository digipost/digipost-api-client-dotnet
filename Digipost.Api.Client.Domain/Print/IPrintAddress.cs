namespace Digipost.Api.Client.Domain.Print
{
    public interface IPrintAddress
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