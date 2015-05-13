using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "invalid-reason", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("invalid-reason", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum InvalidReason
    {
        /// <remarks />
        INVALID_PERSONAL_IDENTIFICATION_NUMBER,

        /// <remarks />
        INVALID_ORGANISATION_NUMBER,

        /// <remarks />
        UNKNOWN
    }
}