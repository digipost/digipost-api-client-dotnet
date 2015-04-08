using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // AuthenticationLevel
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "authentication-level", Namespace = "http://api.digipost.no/schema/v6")]
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
