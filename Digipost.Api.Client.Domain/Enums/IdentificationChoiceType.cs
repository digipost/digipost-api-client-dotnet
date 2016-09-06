using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum IdentificationChoiceType
    {
        /// <summary>
        ///     Digipost address. Issued by Digipost. eg. firstname.surname#id01. Unique per person.
        /// </summary>
        DigipostAddress,

        /// <summary>
        ///     Name and Address of recipient. look at NameAndAddress.cs for more info.
        /// </summary>
        NameAndAddress,

        /// <summary>
        ///     Organisation number. A nine digit registration number issued by the goverment. Unique per organisation.
        /// </summary>
        OrganisationNumber,

        /// <summary>
        ///     Social security number. A twelve digit number issued by the goverment. Unique per person.
        /// </summary>
        PersonalidentificationNumber
    }
}