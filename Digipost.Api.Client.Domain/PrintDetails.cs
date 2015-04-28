using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "print-details", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-details", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class PrintDetails
    {
        private Printcolors _color;
        private NondeliverableHandling _nondeliverableHandling;
        private Posttype _postType;
        private PrintRecipient _recipient;
        private PrintRecipient _returnAddress;

        public PrintDetails()
        {
            _returnAddress = new PrintRecipient();
            _recipient = new PrintRecipient();
            _color = Printcolors.Monochrome;
            _nondeliverableHandling = NondeliverableHandling.ReturnToSender;
        }

        [XmlElement("recipient")]
        public PrintRecipient Recipient
        {
            get { return _recipient; }
            set { _recipient = value; }
        }

        [XmlElement("return-address")]
        public PrintRecipient ReturnAddress
        {
            get { return _returnAddress; }
            set { _returnAddress = value; }
        }

        [XmlElement("post-type")]
        public Posttype PostType
        {
            get { return _postType; }
            set { _postType = value; }
        }

        [DefaultValue(Printcolors.Monochrome)]
        [XmlElement("color")]
        public Printcolors Color
        {
            get { return _color; }
            set { _color = value; }
        }

        [XmlElement("nondeliverable-handling")]
        [DefaultValue(NondeliverableHandling.ReturnToSender)]
        public NondeliverableHandling NondeliverableHandling
        {
            get { return _nondeliverableHandling; }
            set { _nondeliverableHandling = value; }
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "print-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class PrintRecipient
    {
        private object _itemField;
        private string _nameField;

        [XmlElement("name")]
        public string Name
        {
            get { return _nameField; }
            set { _nameField = value; }
        }

        [XmlElement("foreign-address", typeof (Foreignaddress))]
        [XmlElement("norwegian-address", typeof (NorwegianAddress))]
        public object Item
        {
            get { return _itemField; }
            set { _itemField = value; }
        }
    }

    [GeneratedCode("System.Xml", "4.6.42.0")]
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "foreign-address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("foreign-address", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class Foreignaddress
    {
        private string _addressline1;
        private string _addressline2;
        private string _addressline3;
        private string _addressline4;
        private string _item;
        private ItemChoiceType3 _itemElementName;

        [XmlElement("addressline1")]
        public string Addressline1
        {
            get { return _addressline1; }
            set { _addressline1 = value; }
        }

        [XmlElement("addressline2")]
        public string Addressline2
        {
            get { return _addressline2; }
            set { _addressline2 = value; }
        }

        [XmlElement("addressline3")]
        public string Addressline3
        {
            get { return _addressline3; }
            set { _addressline3 = value; }
        }

        [XmlElement("addressline4")]
        public string Addressline4
        {
            get { return _addressline4; }
            set { _addressline4 = value; }
        }

        [XmlElement("country", typeof (string))]
        [XmlElement("country-code", typeof (string))]
        [XmlChoiceIdentifier("ItemElementName")]
        public string Item
        {
            get { return _item; }
            set { _item = value; }
        }

        [XmlIgnore]
        public ItemChoiceType3 ItemElementName
        {
            get { return _itemElementName; }
            set { _itemElementName = value; }
        }
    }

    [GeneratedCode("System.Xml", "4.6.42.0")]
    [Serializable]
    [XmlType(Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum ItemChoiceType3
    {
        /// <remarks />
        [XmlEnum("country")] Country,

        /// <remarks />
        [XmlEnum("country-code")] Countrycode
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "norwegian-address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("norwegian-address", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class NorwegianAddress
    {
        private string _addressline1;
        private string _addressline2;
        private string _addressline3;
        private string _city;
        private string _zipCode;

        [XmlElement("addressline1")]
        public string Addressline1
        {
            get { return _addressline1; }
            set { _addressline1 = value; }
        }

        [XmlElement("addressline2")]
        public string Addressline2
        {
            get { return _addressline2; }
            set { _addressline2 = value; }
        }

        [XmlElement("addressline3")]
        public string Addressline3
        {
            get { return _addressline3; }
            set { _addressline3 = value; }
        }

        [XmlElement("zip-code")]
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        [XmlElement("city")]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
    }

    [Serializable]
    [XmlType(TypeName = "post-type", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("post-type", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum Posttype
    {
        /// <remarks />
        A,

        /// <remarks />
        B
    }


    [Serializable]
    [XmlType(TypeName = "print-colors", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-colors", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum Printcolors
    {
        /// <remarks />
        [XmlEnum("MONOCHROME")] Monochrome,

        /// <remarks />
        [XmlEnum("COLORS")] Colors
    }


    [Serializable]
    [XmlType(TypeName = "nondeliverable-handling", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("nondeliverable-handling", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum NondeliverableHandling
    {
        /// <remarks />
        [XmlEnum("SHRED")] Shred,

        /// <remarks />
        [XmlEnum("RETURN_TO_SENDER")] ReturnToSender
    }
}