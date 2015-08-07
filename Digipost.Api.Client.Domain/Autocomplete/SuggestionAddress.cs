using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Autocomplete
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class SuggestionAddress
    {
        [XmlElement("street")]
        public string Street { get; set; }

        [XmlElement("house-number")]
        public string HouseNumber { get; set; }

        [XmlElement("house-letter")]
        public string HouseLetter { get; set; }

        [XmlElement("additional-addressline")]
        public string AdditionalAddressLine { get; set; }

        [XmlElement("zip-code")]
        public string  ZipCode { get; set; }

        [XmlElement("city")]
        public string City { get; set; }

    }
}
