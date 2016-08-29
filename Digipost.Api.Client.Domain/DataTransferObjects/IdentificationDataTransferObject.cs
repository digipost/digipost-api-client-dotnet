using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    /// <summary>
    ///     Used to identify users in Digipost, by Digipost address, name and address,
    ///     Organization Number or Social Security Number.
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "identification", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6",IsNullable = false)]
    public class IdentificationDataTransferObject
    {
        /// <summary>
        ///     Identify if recipient exists in Digipost by Digipost address, Organization
        ///     Number or Social Security Number.
        /// </summary>
        public IdentificationDataTransferObject(IdentificationChoiceType identificationChoiceType, string value)
        {
            IdentificationValue = value;
            IdentificationType = identificationChoiceType;
        }

        /// <summary>
        ///     Identify if recipient exists in Digipost by name and address.
        /// </summary>
        public IdentificationDataTransferObject(RecipientByNameAndAddressDataTranferObject recipientByNameAndAddressDataTranferObject)
        {
            IdentificationValue = recipientByNameAndAddressDataTranferObject;
            IdentificationType = IdentificationChoiceType.NameAndAddress;
        }

        private IdentificationDataTransferObject()
        {
            /**Must exist for serialization.**/
        }

        [XmlElement("digipost-address", typeof (string))]
        [XmlElement("name-and-address", typeof (RecipientByNameAndAddressDataTranferObject))]
        [XmlElement("organisation-number", typeof (string))]
        [XmlElement("personal-identification-number", typeof (string))]
        [XmlChoiceIdentifier("IdentificationType")]
        public object IdentificationValue { get; set; }

        [XmlIgnore]
        public IdentificationChoiceType IdentificationType { get; set; }
    }
}