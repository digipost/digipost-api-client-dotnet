using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType("ItemChoiceType", Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum IdentificationResultType
    {
        /// <remarks />
        [XmlEnum("digipost-address")] Digipostaddress,

        /// <remarks />
        [XmlEnum("invalid-reason")] Invalidreason,

        /// <remarks />
        [XmlEnum("person-alias")] Personalias,

        /// <remarks />
        [XmlEnum("unidentified-reason")] Unidentifiedreason
    }
}