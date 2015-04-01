using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    //IdentificationChoice
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum IdentificationChoice
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("digipost-address")]
        digipostaddress,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("name-and-address")]
        nameandaddress,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("organisation-number")]
        organisationnumber,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("personal-identification-number")]
        personalidentificationnumber,
    }
}
