using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Autocomplete
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class SuggestionRecipient
    {
        [XmlElement("firstname")]
        public string FirstName { get; set; }

        [XmlElement("middlename")]
        public string MiddleName { get; set; }

        [XmlElement("lastname")]
        public string LastName { get; set; }

        [XmlElement("digipost-address")]
        public string DigipostAddress { get; set; }

        [XmlElement("mobile-number")]
        public string MobileNumber { get; set; }

        [XmlElement("organisation-name")]
        public string OrganizationName { get; set; }

        [XmlElement("address")]
        public SuggestionAddress SuggestionAddress{ get; set; }
    }
}
