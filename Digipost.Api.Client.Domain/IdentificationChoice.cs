using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    //IdentificationChoice
    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [XmlType(Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum IdentificationChoice
    {
        /// <remarks />
        [XmlEnum("digipost-address")] Digipostaddress,

        /// <remarks />
        [XmlEnum("name-and-address")] Nameandaddress,

        /// <remarks />
        [XmlEnum("organisation-number")] Organisationnumber,

        /// <remarks />
        [XmlEnum("personal-identification-number")] Personalidentificationnumber
    }
}