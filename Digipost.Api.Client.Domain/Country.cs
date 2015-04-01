using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum Country
    {

        /// <remarks/>
        country,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("country-code")]
        countrycode,
    }
}
