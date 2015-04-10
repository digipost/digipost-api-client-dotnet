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
        [XmlEnum("digipost-address")] DigipostAddress,

        /// <remarks />
        [XmlEnum("name-and-address")] NameAndAddress,

        /// <remarks />
        [XmlEnum("organisation-number")] OrganisationNumber,

        /// <remarks />
        [XmlEnum("personal-identification-number")] PersonalidentificationNumber
    }
}