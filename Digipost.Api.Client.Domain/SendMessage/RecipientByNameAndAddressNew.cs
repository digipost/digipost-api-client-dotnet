using System;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientByNameAndAddressNew : IRecipient
    {
        public RecipientByNameAndAddressNew(string fullName, string postalCode, string city, string addressLine1, IPrintDetails printDetails = null)
        {
            FullName = fullName;
            PostalCode = postalCode;
            City = city;
            AddressLine1 = addressLine1;
            PrintDetails = printDetails;
        }

        public object IdentificationValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IPrintDetails PrintDetails { get; set; }

        public IdentificationChoiceType? IdentificationType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string FullName { get; set; }

        public string PostalCode { get; set; }
        
        public string City { get; set; }
        
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public DateTime? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
