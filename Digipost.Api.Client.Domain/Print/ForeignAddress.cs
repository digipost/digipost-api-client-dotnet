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
    public class ForeignAddress
    {
        private ForeignAddress()
        {
            /**must exist for serializing**/
        }

        public ForeignAddress(CountryIdentifier countryIdentifier, string countryIdentifierValue,
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

        /// <summary>
        ///     The value of the contryIdentifier. Either Country or Country-code
        /// </summary>
        [XmlElement("country", typeof (string))]
        [XmlElement("country-code", typeof (string))]
        [XmlChoiceIdentifier("CountryIdentifier")]
        public string CountryIdentifierValue { get; set; }

        /// <summary>
        ///     Choose which how you will identify the country.
        /// </summary>
        [XmlIgnore]
        public CountryIdentifier CountryIdentifier { get; set; }
    }
}