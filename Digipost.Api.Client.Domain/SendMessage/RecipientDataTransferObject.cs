using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "message-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class RecipientDataTransferObject
    {
        private RecipientDataTransferObject()
        {
            /**Must exist for serialization.**/
        }

        public RecipientDataTransferObject(RecipientByNameAndAddress recipientByNameAndAddress, PrintDetails printDetails = null)
        {
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoiceType.NameAndAddress;
            PrintDetails = printDetails;
        }

        public RecipientDataTransferObject(IdentificationChoiceType identificationChoiceType, string id, PrintDetails printDetails = null)
        {
            if (identificationChoiceType == IdentificationChoiceType.NameAndAddress)
                throw new ArgumentException(string.Format("Not allowed to set identification choice by {0} " +
                                                          "when using string as id",
                    IdentificationChoiceType.NameAndAddress));
            IdentificationValue = id;
            IdentificationType = identificationChoiceType;
            PrintDetails = printDetails;
        }
        
        public RecipientDataTransferObject(PrintDetails printDetails)
        {
            PrintDetails = printDetails;
        }

        [XmlElement("digipost-address", typeof (string))]
        [XmlElement("name-and-address", typeof (RecipientByNameAndAddress))]
        [XmlElement("organisation-number", typeof (string))]
        [XmlElement("personal-identification-number", typeof (string))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        [XmlElement("print-details")]
        public PrintDetails PrintDetails { get; set; }

        [Obsolete("Will be renamed to IdentificationChoiceType. NB. This will be removed in future version.")] //With next breaking change, rename to IdenticicationChoiceType.
        [XmlIgnore]
        public IdentificationChoiceType IdentificationType { get;  set; }
    }
}