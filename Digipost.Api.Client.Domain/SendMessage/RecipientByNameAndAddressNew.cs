using System;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientByNameAndAddressNew : IRecipient
    {
        public RecipientByNameAndAddressNew(string fullName, string postalCode, string city, string addressLine, IPrintDetails printDetails = null)
        {
            FullName = fullName;
            PostalCode = postalCode;
            City = city;
            AddressLine = addressLine;
            PrintDetails = printDetails;
        }
        
        public object IdentificationValue { get; set; }
        
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


        public IdentificationType Identificationtype { get; set; }

        public string FullName { get; set; }

        public string PostalCode { get; set; }
        
        public string City { get; set; }
        
        public string AddressLine { get; set; }
    }
}
