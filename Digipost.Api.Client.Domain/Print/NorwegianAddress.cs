using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Print
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "norwegian-address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("norwegian-address", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class NorwegianAddress
    {
        [XmlElement("addressline1")]
        public string Addressline1 { get; set; }

        [XmlElement("addressline2")]
        public string Addressline2 { get; set; }

        [XmlElement("addressline3")]
        public string Addressline3 { get; set; }

        [XmlElement("zip-code")]
        public string ZipCode { get; set; }

        [XmlElement("city")]
        public string City { get; set; }
    }
}
