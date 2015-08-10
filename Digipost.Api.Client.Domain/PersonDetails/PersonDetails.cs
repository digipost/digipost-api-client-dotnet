using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.PersonDetails
{
    /// <summary>
    /// When requesting person information via the PersonDetails API, this class is the result.
    /// </summary>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class PersonDetails
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
        public PersonDetailsAddress SuggestionAddress{ get; set; }
    }
}
