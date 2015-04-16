using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [XmlType(TypeName = "name-and-address", Namespace = "http://api.digipost.no/schema/v6")]
    public class NameAndAddress
    {
        [XmlElement("fullname")]
        public string FullName { get; set; }

        [XmlElement("addressline1")]
        public string AddressLine1 { get; set; }

        [XmlElement("addressline2")]
        public string Addressline2 { get; set; }

        [XmlElement("postalcode")]
        public string Postalcode { get; set; }

        [XmlElement("city")]
        public string City { get; set; }

        [XmlElement("birth-date", DataType = "date")]
        public DateTime Birthdate { get; set; }

        [XmlIgnore]
        public bool BirthdateSpecified { get; set; }

        [XmlElement("phone-number")]
        public string Phonenumber { get; set; }

        [XmlElement("email-address")]
        public string EmailAddress { get; set; }
    }
}