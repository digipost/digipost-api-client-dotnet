using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.SendMessage
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "name-and-address", Namespace = "http://api.digipost.no/schema/v6")]
    public class RecipientByNameAndAddressDataTranferObject : IRecipientByNameAndAddress
    {
        private RecipientByNameAndAddressDataTranferObject()
        {
            /**Must exist for serialization.**/
            BirthDate = null;
        }

        public RecipientByNameAndAddressDataTranferObject(string fullName, string postalCode, string city, string addressLine1)
        {
            FullName = fullName;
            AddressLine1 = addressLine1;
            PostalCode = postalCode;
            City = city;
        }

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
        public DateTime? BirthDate { get; set; }

        [XmlElement("phone-number")]
        public string PhoneNumber { get; set; }

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