using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "unidentified-reason", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("unidentified-reason", Namespace = "http://api.digipost.no/schema/v6",IsNullable = false)]
    public enum UnidentifiedReason
    {
        /// <summary>
        ///     When more than one possible subject. Try narrow down the search with more information about the subject.
        /// </summary>
        [XmlEnum("MULTIPLE_MATCHES")]
        MultipleMatches,

        /// <summary>
        ///     Subject not found based on search criteria.
        /// </summary>
        [XmlEnum("NOT_FOUND")]
        NotFound
    }
}