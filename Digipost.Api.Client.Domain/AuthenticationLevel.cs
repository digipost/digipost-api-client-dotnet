using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <remarks/>
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable()]
    [XmlType(TypeName = "authentication-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum AuthenticationLevel
    {

        /// <remarks/>
        [XmlEnum("PASSWORD")]
        Password,

        /// <remarks/>
        [XmlEnum("TWO_FACTOR")]
        TwoFactor,

        /// <remarks/>
        [XmlEnum("IDPORTEN_3")]
        IdPorten_3,

        /// <remarks/>
        [XmlEnum("IDPORTEN_4")]
        IdPorten_4,
    }
}
