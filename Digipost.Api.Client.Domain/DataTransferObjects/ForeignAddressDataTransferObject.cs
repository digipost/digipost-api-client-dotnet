using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Print
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "foreign-address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("foreign-address", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class ForeignAddressDataTransferObject
    {
        private ForeignAddressDataTransferObject()
        {
            /**must exist for serializing**/
        }

        /// <summary>
        /// Foreign address for use when sending a letter abroad.
        /// </summary>
        /// <param name="countryIdentifier">Type of identifier used for identifying a country.</param>
        /// <param name="countryIdentifierValue">The value for country.</param>
        /// <param name="addressLine1">First address line.</param>
        /// <param name="addressLine2">Second address line.</param>
        /// <param name="addressLine3">Third address line.</param>
        /// <param name="addressLine4">Fourth address line.</param>
        public ForeignAddressDataTransferObject(CountryIdentifier countryIdentifier, string countryIdentifierValue,
            string addressLine1, string addressLine2 = null, string addressLine3 = null, string addressLine4 = null)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressLine3 = addressLine3;
            AddressLine4 = addressLine4;
            CountryIdentifier = countryIdentifier;
            CountryIdentifierValue = countryIdentifierValue;
        }

        [XmlElement("addressline1")]
        public string AddressLine1 { get; set; }

        [XmlElement("addressline2")]
        public string AddressLine2 { get; set; }

        [XmlElement("addressline3")]
        public string AddressLine3 { get; set; }

        [XmlElement("addressline4")]
        public string AddressLine4 { get; set; }

        [XmlElement("country", typeof (string))]
        [XmlElement("country-code", typeof (string))]
        [XmlChoiceIdentifier("CountryIdentifier")]
        public string CountryIdentifierValue { get; set; }

        [XmlIgnore]
        public CountryIdentifier CountryIdentifier { get; set; }
    }
}