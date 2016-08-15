using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "CountryIdentifier", Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum CountryIdentifier
    {
        /// <summary>
        ///     Country name in Norwegian or English.
        /// </summary>
        [XmlEnum("country")]
        Country,

        /// <summary>
        ///     Country code according to the ISO 3166-1 alpha-2 standard.
        /// </summary>

        [XmlEnum("country-code")]
        Countrycode
    }
}