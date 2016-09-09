using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientByNameAndAddressDataTranferObject : IRecipientByNameAndAddress
    {
        public RecipientByNameAndAddressDataTranferObject(string fullName, string postalCode, string city, string addressLine1)
        {
            FullName = fullName;
            AddressLine1 = addressLine1;
            PostalCode = postalCode;
            City = city;
        }

        public string FullName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public DateTime? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}