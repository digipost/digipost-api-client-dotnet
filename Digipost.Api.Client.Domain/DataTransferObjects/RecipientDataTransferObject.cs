using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.DataTransferObjects
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

        public RecipientDataTransferObject(RecipientByNameAndAddress recipientByNameAndAddress, PrintDetailsDataTransferObject printDetailsDataTransferObject = null)
        {
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoiceType.NameAndAddress;
            PrintDetailsDataTransferObject = printDetailsDataTransferObject;
        }

        public RecipientDataTransferObject(IdentificationChoiceType identificationChoiceType, string id, PrintDetailsDataTransferObject printDetailsDataTransferObject = null)
        {
            if (identificationChoiceType == IdentificationChoiceType.NameAndAddress)
                throw new ArgumentException(string.Format("Not allowed to set identification choice by {0} " +
                                                          "when using string as id",
                    IdentificationChoiceType.NameAndAddress));
            IdentificationValue = id;
            IdentificationType = identificationChoiceType;
            PrintDetailsDataTransferObject = printDetailsDataTransferObject;
        }
        
        public RecipientDataTransferObject(PrintDetailsDataTransferObject printDetailsDataTransferObject)
        {
            PrintDetailsDataTransferObject = printDetailsDataTransferObject;
        }

        [XmlElement("digipost-address", typeof (string))]
        [XmlElement("name-and-address", typeof (RecipientByNameAndAddress))]
        [XmlElement("organisation-number", typeof (string))]
        [XmlElement("personal-identification-number", typeof (string))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        [XmlElement("print-details")]
        public PrintDetailsDataTransferObject PrintDetailsDataTransferObject { get; set; }

        [XmlIgnore]
        public IdentificationChoiceType IdentificationType { get;  set; }
    }
}