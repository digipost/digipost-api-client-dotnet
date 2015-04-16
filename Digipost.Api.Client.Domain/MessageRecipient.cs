using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class MessageRecipient
    {
        [XmlElement("digipost-address", typeof (string))]
        [XmlElement("name-and-address", typeof (NameAndAddress))]
        [XmlElement("organisation-number", typeof (string))]
        [XmlElement("personal-identification-number", typeof (string))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        public MessageRecipient() { /**Must exist for serialization.**/ }

        public MessageRecipient(NameAndAddress nameAndAddress)
        {
            IdentificationValue = nameAndAddress;
            IdentificationType = IdentificationChoice.NameAndAddress;
        }

        public MessageRecipient(IdentificationChoice identificationChoice, string id)
        {
            IdentificationValue = id;
            IdentificationType = identificationChoice;
        }

        [XmlIgnore]
        public IdentificationChoice IdentificationType { get; private set; }
    }
}