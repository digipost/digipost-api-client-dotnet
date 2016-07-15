using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType("ItemChoiceType", Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum IdentificationResultType
    {
        [XmlEnum("")]
        None,

        [XmlEnum("digipost-address")]
        DigipostAddress,

        [XmlEnum("invalid-reason")]
        InvalidReason,

        [XmlEnum("person-alias")]
        Personalias,

        [XmlEnum("unidentified-reason")]
        UnidentifiedReason
    }
}