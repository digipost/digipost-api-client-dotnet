using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "unidentified-reason", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("unidentified-reason", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum UnidentifiedReason
    {
        /// <remarks />
        MULTIPLE_MATCHES,

        /// <remarks />
        NOT_FOUND
    }
}