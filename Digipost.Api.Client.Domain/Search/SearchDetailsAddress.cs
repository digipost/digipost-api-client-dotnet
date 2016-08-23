using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Search
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6",IsNullable = false)]
    public class SearchDetailsAddress : ISearchDetailsAddress
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
        public string ZipCode { get; set; }

        [XmlElement("city")]
        public string City { get; set; }

        public override string ToString()
        {
            return string.Format("Street: {0}, HouseNumber: {1}, HouseLetter: {2}, AdditionalAddressLine: {3}, ZipCode: {4}, City: {5}", Street, HouseNumber, HouseLetter, AdditionalAddressLine, ZipCode, City);
        }
    }
}