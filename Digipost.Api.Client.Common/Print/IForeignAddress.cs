using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Print
{
    public interface IForeignAddress : IAddress
    {
        string Addressline4 { get; set; }

        /// <summary>
        ///     The value of the contryIdentifier.
        /// </summary>
        string CountryIdentifierValue { get; set; }

        /// <summary>
        ///     Choose how to will identify the country.
        /// </summary>
        CountryIdentifier CountryIdentifier { get; set; }
    }
}