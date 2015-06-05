using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "name-and-address", Namespace = "http://api.digipost.no/schema/v6")]
    public class RecipientByNameAndAddress
    {
        private RecipientByNameAndAddress()
        {
            /**Must exist for serialization.**/
            BirthDate = null;
        }

        public RecipientByNameAndAddress(string fullName, string postalCode, string city, string addressLine)
        {
            FullName = fullName;
            AddressLine1 = addressLine;
            PostalCode = postalCode;
            City = city;
        }

        /// <summary>
        ///     Full name of person, first name first.
        /// </summary>
        [XmlElement("fullname")]
        public string FullName { get; set; }

        /// <summary>
        ///     Primary address
        /// </summary>
        [XmlElement("addressline1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        ///     Secondary addressline
        /// </summary>
        [XmlElement("addressline2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        ///     Postal code
        /// </summary>
        [XmlElement("postalcode")]
        public string PostalCode { get; set; }

        /// <summary>
        ///     City
        /// </summary>
        [XmlElement("city")]
        public string City { get; set; }

        /// <summary>
        ///     Birth date
        /// </summary>
        [XmlElement("birth-date", DataType = "date")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        ///     Phone number
        /// </summary>
        [XmlElement("phone-number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     E-mail address
        /// </summary>
        [XmlElement("email-address")]
        public string Email { get; set; }

        public bool ShouldSerializeBirthDate()
        {
            /** Note: This field must have same name as BirthDate with prefix ShouldSerialize in order to work.
                Is recognized by framework, as seen in: 
                http://stackoverflow.com/questions/1296468/suppress-null-value-types-from-being-emitted-by-xmlserializer
             **/
            return BirthDate != null;
        }
    }
}