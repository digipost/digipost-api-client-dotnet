using System;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientByNameAndAddress : DigipostRecipient, IRecipientByNameAndAddress
    {
        public RecipientByNameAndAddress(string fullName, string addressLine1, string postalCode, string city)
        {
            FullName = fullName;
            PostalCode = postalCode;
            City = city;
            AddressLine1 = addressLine1;
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