using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum IdentificationChoiceType
    {
        /// <summary>
        ///     Digipost address. Issued by Digipost. eg. firstname.surname#id01. Unique per person.
        /// </summary>
        [XmlEnum("digipost-address")]
        DigipostAddress,

        /// <summary>
        ///     Name and Address of recipient. look at NameAndAddress.cs for more info.
        /// </summary>
        [XmlEnum("name-and-address")]
        NameAndAddress,

        /// <summary>
        ///     Organisation number. A nine digit registration number issued by the goverment. Unique per organisation.
        /// </summary>
        [XmlEnum("organisation-number")]
        OrganisationNumber,

        /// <summary>
        ///     Social security number. A twelve digit number issued by the goverment. Unique per person.
        /// </summary>
        [XmlEnum("personal-identification-number")]
        PersonalidentificationNumber
    }
}