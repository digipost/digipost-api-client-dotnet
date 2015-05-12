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
    [XmlType(TypeName = "identification-result", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class IdentificationResult
    {
        [XmlElement("result")]
        public IdentificationResultCode IdentificationResultCode { get; set; }

        [XmlElement("digipost-address", typeof(string))]
        [XmlElement("invalid-reason", typeof(InvalidReason))]
        [XmlElement("person-alias", typeof(string))]
        [XmlElement("unidentified-reason", typeof(UnidentifiedReason))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        [XmlIgnore]
        public IdentificationResultType IdentificationType { get; set; }

   

    }
}
