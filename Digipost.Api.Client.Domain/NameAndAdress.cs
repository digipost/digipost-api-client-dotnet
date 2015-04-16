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
    [XmlType(TypeName = "name-and-address", Namespace = "http://api.digipost.no/schema/v6")]
    public class NameAndAddress
    {
        [XmlElement("fullname")]
        public string FullName { get; set; }

        [XmlElement("addressline1")]
        public string AddressLine1 { get; set; }

        [XmlElement("addressline2")]
        public string AddressLine2 { get; set; }

        [XmlElement("postalcode")]
        public string PostalCode { get; set; }

        [XmlElement("city")]
        public string City { get; set; }

        [XmlElement("birth-date", DataType = "date")]
        public DateTime BirthDate { get; set; }

        [XmlIgnore]
        public bool IsBirthDateSpecified { get; set; }

        [XmlElement("phone-number")]
        public string PhoneNumber { get; set; }

        [XmlElement("email-address")]
        public string Email { get; set; }
    }
}