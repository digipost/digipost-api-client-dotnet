using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [XmlType(TypeName = "authentication-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum AuthenticationLevel
    {
        [XmlEnum("PASSWORD")]
        Password,

        [XmlEnum("TWO_FACTOR")]
        TwoFactor,

        [XmlEnum("IDPORTEN_3")]
        IdPorten3,

        [XmlEnum("IDPORTEN_4")]
        IdPorten4,
    }
}
