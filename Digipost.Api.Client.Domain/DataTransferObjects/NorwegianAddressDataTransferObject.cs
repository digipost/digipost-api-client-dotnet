using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "norwegian-address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("norwegian-address", Namespace = "http://api.digipost.no/schema/v6",IsNullable = true)]
    public class NorwegianAddressDataTransferObject
    {
        private NorwegianAddressDataTransferObject()
        {
            /**must exist for serializing**/
        }

        /// <summary>
        ///     Norwegian address for use when sending a letter within Norway.
        /// </summary>
        /// <param name="postalCode">Postal code for the address provided.</param>
        /// <param name="city">City in which the address resides.</param>
        /// <param name="addressline1">First address line.</param>
        /// <param name="addressline2">Second address line.</param>
        /// <param name="addressline3">Third address line.</param>
        public NorwegianAddressDataTransferObject(string postalCode,
            string city, string addressline1, string addressline2 = null, string addressline3 = null)
        {
            Addressline1 = addressline1;
            Addressline2 = addressline2;
            Addressline3 = addressline3;
            City = city;
            PostalCode = postalCode;
        }

        [XmlElement("addressline1")]
        public string Addressline1 { get; set; }

        [XmlElement("addressline2")]
        public string Addressline2 { get; set; }

        [XmlElement("addressline3")]
        public string Addressline3 { get; set; }

        [XmlElement("zip-code")]
        public string PostalCode { get; set; }

        [XmlElement("city")]
        public string City { get; set; }
    }
}