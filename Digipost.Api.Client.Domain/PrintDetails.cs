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
        public PrintDetails()
        {
            ReturnAddress = new PrintRecipient();
            Recipient = new PrintRecipient();
            Color = Printcolors.Monochrome;
            NondeliverableHandling = NondeliverableHandling.ReturnToSender;
        }

        [XmlElement("recipient")]
        public PrintRecipient Recipient { get; set; }

        [XmlElement("return-address")]
        public PrintRecipient ReturnAddress { get; set; }

        [XmlElement("post-type")]
        public Posttype PostType { get; set; }

        [DefaultValue(Printcolors.Monochrome)]
        [XmlElement("color")]
        public Printcolors Color { get; set; }

        [XmlElement("nondeliverable-handling")]
        [DefaultValue(NondeliverableHandling.ReturnToSender)]
        public NondeliverableHandling NondeliverableHandling { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "print-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class PrintRecipient
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("foreign-address", typeof (Foreignaddress))]
        [XmlElement("norwegian-address", typeof (NorwegianAddress))]
        public object Address { get; set; }
    }

    [GeneratedCode("System.Xml", "4.6.42.0")]
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "foreign-address", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("foreign-address", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class Foreignaddress
    {
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
        [XmlChoiceIdentifier("ItemElementName")]
        public string CountryIdentifierValue { get; set; }

        [XmlIgnore]
        public CountryIdentifier CountryIdentifier { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://api.digipost.no/schema/v6", IncludeInSchema = false)]
    public enum CountryIdentifier
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

    [Serializable]
    [XmlType(TypeName = "post-type", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("post-type", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum Posttype
    {
        /// <remarks />
        [XmlEnum("A")] A,

        /// <remarks />
        [XmlEnum("B")] B
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