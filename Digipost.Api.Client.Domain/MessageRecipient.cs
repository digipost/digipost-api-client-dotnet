using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class MessageRecipient
    {
        private IdentificationChoice _identification;
        private object _identificationValue;

        /// <remarks />
        [XmlElement("digipost-address", typeof (string))]
        [XmlElement("name-and-address", typeof (NameAndAddress))]
        [XmlElement("organisation-number", typeof (string))]
        [XmlElement("personal-identification-number", typeof (string))]
        [XmlChoiceIdentifier("ItemElementName")]
        public object Identification
        {
            get { return _identificationValue; }
            set { _identificationValue = value; }
        }

        /// <remarks />
        [XmlIgnore]
        public IdentificationChoice ItemElementName
        {
            get { return _identification; }
            set { _identification = value; }
        }
    }
}