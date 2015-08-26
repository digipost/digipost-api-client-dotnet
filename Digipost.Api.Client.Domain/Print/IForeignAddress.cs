using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Print
{
    public interface IForeignAddress
    {
        string Addressline1 { get; set; }
        string Addressline2 { get; set; }
        string Addressline3 { get; set; }
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