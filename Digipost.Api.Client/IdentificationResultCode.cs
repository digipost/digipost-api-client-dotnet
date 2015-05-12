using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client
{
    [Serializable]
    [XmlType(TypeName = "identification-result-code", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum IdentificationResultCode
    {
        [XmlElement("DIGIPOST")]
        Digipost,

        [XmlElement("IDENTIFIED")]
        Identified,

        [XmlElement("UNIDENTIFIED")]
        Unidentified,

        [XmlElement("INVALID")]
        Invalid
    }
}
