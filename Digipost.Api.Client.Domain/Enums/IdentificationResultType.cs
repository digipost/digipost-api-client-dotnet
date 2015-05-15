using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType("ItemChoiceType", Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum IdentificationResultType
    {
        /// <summary>
        /// The subjects digipost-address
        /// </summary>
        [XmlEnum("digipost-address")] 
        Digipostaddress,

        /// <summary>
        /// Enum of invalidreason
        /// </summary>
        [XmlEnum("invalid-reason")] 
        Invalidreason,

        /// <summary>
        /// The subjects personalias.
        /// </summary>
        [XmlEnum("person-alias")] 
        Personalias,

        /// <summary>
        /// Enum of unidentified reason.
        /// </summary>
        [XmlEnum("unidentified-reason")] 
        Unidentifiedreason
    }
}