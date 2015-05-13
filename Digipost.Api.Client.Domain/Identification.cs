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

        [XmlElement("digipost-address", typeof(string))]
        [XmlElement("name-and-address", typeof(RecipientByNameAndAddress))]
        [XmlElement("organisation-number", typeof(string))]
        [XmlElement("personal-identification-number", typeof(string))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        [XmlIgnore]
        public IdentificationChoice IdentificationType { get; set; }
    }
}
