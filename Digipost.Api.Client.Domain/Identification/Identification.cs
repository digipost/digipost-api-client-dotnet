using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public class Identification : IIdentification
    {
        public IdentificationChoice IdentificationChoice { get; set; }
        
        public Identification(IdentificationChoice identificationChoice, string value)
        {
            IdentificationChoice = identificationChoice;
            Data = value;
        }

        public Identification(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            Data = recipientByNameAndAddress;
        }
        
        public object Data { get; internal set; }

    }
}
