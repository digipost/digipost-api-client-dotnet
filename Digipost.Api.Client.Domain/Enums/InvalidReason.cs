using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "invalid-reason", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("invalid-reason", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum InvalidReason
    {
        /// <summary>
        /// Invalid Social Security Number (SSN). Check the number and try again.
        /// </summary>
        [XmlEnum("INVALID_PERSONAL_IDENTIFICATION_NUMBER")]
        InvalidPersonalIdentificationNumber,

        /// <summary>
        /// Invalid organisation number. Check the number and try again.
        /// </summary>
        [XmlEnum("INVALID_ORGANISATION_NUMBER")]
        InvalidOrganisationNumber,

        /// <summary>
        /// Subject is unknown.
        /// </summary>
        [XmlEnum("UNKNOWN")]
        Unknown
    }
}