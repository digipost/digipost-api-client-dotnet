using System;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientById : IRecipient
    {
        public RecipientById(IdentificationType identificationType, string id, PrintDetails printDetails = null)
        {
            Identificationtype = identificationType;
            Id = id;
            IdentificationValue = id;
            PrintDetails = printDetails;
        }

        public string Id { get; set; }
        
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
    }
}
