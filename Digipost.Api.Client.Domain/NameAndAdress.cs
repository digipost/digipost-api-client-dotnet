using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "name-and-address", Namespace = "http://api.digipost.no/schema/v6")]
    public class NameAndAddress
    {
        private string _addressLine1;
        private string _addressLine2;
        private DateTime _birthdate;
        private bool _birthdateSpecified;
        private string _city;
        private string _emailAddress;
        private string _fullName;
        private string _phonenumber;
        private string _postalcode;

        /// <remarks />
        [XmlElement("fullname")]
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        /// <remarks />
        [XmlElement("addressline1")]
        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }

        /// <remarks />
        [XmlElement("addressline2")]
        public string Addressline2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }

        /// <remarks />
        [XmlElement("postalcode")]
        public string Postalcode
        {
            get { return _postalcode; }
            set { _postalcode = value; }
        }

        /// <remarks />
        [XmlElement("city")]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <remarks />
        [XmlElement("birth-date", DataType = "date")]
        public DateTime Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }

        /// <remarks />
        [XmlIgnore]
        public bool BirthdateSpecified
        {
            get { return _birthdateSpecified; }
            set { _birthdateSpecified = value; }
        }

        /// <remarks />
        [XmlElement("phone-number")]
        public string Phonenumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value; }
        }

        /// <remarks />
        [XmlElement("email-address")]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }
    }
}