using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    // AuthenticationLevel
    /// <remarks/>
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable()]
    [XmlType(TypeName = "authentication-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum AuthenticationLevel
    {

        /// <remarks/>
        PASSWORD,

        /// <remarks/>
        TWO_FACTOR,

        /// <remarks/>
        IDPORTEN_3,

        /// <remarks/>
        IDPORTEN_4,
    }
}
