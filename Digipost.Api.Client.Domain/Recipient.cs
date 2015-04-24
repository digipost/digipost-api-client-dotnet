using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Recipient
    {
        [XmlElement("digipost-address", typeof (string))]
        [XmlElement("name-and-address", typeof (RecipientByNameAndAddress))]
        [XmlElement("organisation-number", typeof (string))]
        [XmlElement("personal-identification-number", typeof (string))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        private Recipient() { /**Must exist for serialization.**/ }

        public Recipient(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoice.NameAndAddress;
        }

        public Recipient(IdentificationChoice identificationChoice, string id)
        {
            if (identificationChoice == IdentificationChoice.NameAndAddress)		
                throw new ArgumentException(string.Format("Not allowed to set identification choice by {0} " +		
                                                          "when using string as id",		
                                                          IdentificationChoice.NameAndAddress.ToString()));
            IdentificationValue = id;
            IdentificationType = identificationChoice;
        }

        [XmlIgnore]
        public IdentificationChoice IdentificationType { get; private set; }
    }
}