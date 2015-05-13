using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "identification", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Identification : XmlBodyContent
    {

        /// <summary>
        ///     Identify the subject by either digipost-address,organisation number or SSN.
        /// </summary>
        public Identification(IdentificationChoice identificationChoice, string value)
        {
            if (identificationChoice == IdentificationChoice.NameAndAddress)
                throw new ArgumentException(string.Format("Not allowed to set identification choice by {0} " +
                                                          "when using string as id",
                    IdentificationChoice.NameAndAddress.ToString()));
            
            IdentificationValue = value;
            IdentificationType = identificationChoice;
        }

        /// <summary>
        ///     Identify the subject by name and address.
        /// </summary>
        public Identification(IdentificationChoice identificationChoice, RecipientByNameAndAddress recipientByNameAndAddress)
        {
            
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoice.NameAndAddress;   
        }

        /// <summary>
        ///     Identify the subject by name and address.
        /// </summary>
        public Identification( RecipientByNameAndAddress recipientByNameAndAddress)
        {
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoice.NameAndAddress;
        }

        private Identification()
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