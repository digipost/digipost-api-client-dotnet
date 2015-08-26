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
        /// <param name="addressline1">First address line.</param>
        /// <param name="addressline2">Second address line.</param>
        /// <param name="addressline3">Third address line.</param>
        /// <param name="addressline4">Fourth address line.</param>
        public ForeignAddressDataTransferObject(CountryIdentifier countryIdentifier, string countryIdentifierValue,
            string addressline1, string addressline2 = null, string addressline3 = null, string addressline4 = null)
        {
            Addressline1 = addressline1;
            Addressline2 = addressline2;
            Addressline3 = addressline3;
            Addressline4 = addressline4;
            CountryIdentifier = countryIdentifier;
            CountryIdentifierValue = countryIdentifierValue;
        }

        [XmlElement("addressline1")]
        public string Addressline1 { get; set; }

        [XmlElement("addressline2")]
        public string Addressline2 { get; set; }

        [XmlElement("addressline3")]
        public string Addressline3 { get; set; }

        [XmlElement("addressline4")]
        public string Addressline4 { get; set; }

        [XmlElement("country", typeof (string))]
        [XmlElement("country-code", typeof (string))]
        [XmlChoiceIdentifier("CountryIdentifier")]
        public string CountryIdentifierValue { get; set; }

        [XmlIgnore]
        public CountryIdentifier CountryIdentifier { get; set; }
    }
}