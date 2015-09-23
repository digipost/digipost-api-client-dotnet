using System;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientByNameAndAddress : DigipostRecipient, IRecipientByNameAndAddress
    {
        public RecipientByNameAndAddress(string fullName, string postalCode, string city, string addressLine1, IPrintDetails printDetails = null)
        {
            FullName = fullName;
            PostalCode = postalCode;
            City = city;
            AddressLine1 = addressLine1;
            PrintDetails = printDetails;
        }

        public IPrintDetails PrintDetails { get; set; }

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
