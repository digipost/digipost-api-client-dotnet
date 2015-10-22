using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "identification-result-code", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("identification-result-code", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum IdentificationResultCode
    {
        [XmlEnum("DIGIPOST")] 
        Digipost,

        [XmlEnum("IDENTIFIED")] 
        Identified,

        [XmlEnum("UNIDENTIFIED")] 
        Unidentified,

        [XmlEnum("INVALID")] 
        Invalid
    }
}