using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

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

        [XmlIgnore]
        public IdentificationChoice IdentificationType { get; set; }
    }
}