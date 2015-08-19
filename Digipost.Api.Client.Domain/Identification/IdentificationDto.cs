using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    /// <summary>
    /// Used to identify users in Digipost, by Digipost address, name and address, 
    /// Organization Number or Social Security Number.
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "identification", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class IdentificationDto
    {

        /// <summary>
        /// Identify if recipient exists in Digipost by Digipost address, Organization 
        /// Number or Social Security Number.
        /// </summary>
        public IdentificationDto(IdentificationChoice identificationChoice, string value)
        {
            if (identificationChoice == IdentificationChoice.NameAndAddress)
                throw new ArgumentException(string.Format("Not allowed to set identification choice by {0} " +
                                                          "when using string as id",
                    IdentificationChoice.NameAndAddress));
            
            IdentificationValue = value;
            IdentificationType = identificationChoice;
        }

        /// <summary>
        /// Identify if recipient exists in Digipost by name and address.
        /// </summary>
        [Obsolete("Deprecated, this constuctor is not necessary as the IdentificationChoice will not change the result. Note! This will be removed in future version.")]
        public IdentificationDto(IdentificationChoice identificationChoice, RecipientByNameAndAddress recipientByNameAndAddress)
        {
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoice.NameAndAddress;
        }

        /// <summary>
        /// Identify if recipient exists in Digipost by name and address.
        /// </summary>
        public IdentificationDto(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoice.NameAndAddress;
        }

        private IdentificationDto()
        {
            /**Must exist for serialization.**/
        }

        [XmlElement("digipost-address", typeof (string))]
        [XmlElement("name-and-address", typeof (RecipientByNameAndAddress))]
        [XmlElement("organisation-number", typeof (string))]
        [XmlElement("personal-identification-number", typeof (string))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        [XmlIgnore]
        public IdentificationChoice IdentificationType { get; set; }
    }
}