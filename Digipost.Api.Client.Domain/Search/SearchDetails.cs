using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Search
{
    /// <summary>
    /// When requesting person information via the SearchDetails API, this class is the result.
    /// </summary>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class SearchDetails : ISearchDetails
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
        public SearchDetailsAddress SearchDetailsAddress{ get; set; }

        public override string ToString()
        {
            return string.Format("FirstName: {0}, MiddleName: {1}, LastName: {2}, DigipostAddress: {3}, MobileNumber: {4}, OrganizationName: {5}, SearchDetailsAddress: {6}", FirstName, MiddleName, LastName, DigipostAddress, MobileNumber, OrganizationName, SearchDetailsAddress);
        }
    }
}
