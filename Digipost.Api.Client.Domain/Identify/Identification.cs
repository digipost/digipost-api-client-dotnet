using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identify
{
    public class Identification : IIdentification
    {
        public IdentificationChoiceType IdentificationChoiceType { get; set; }
        
        public Identification(IdentificationChoiceType identificationChoiceType, string value)
        {
            IdentificationChoiceType = identificationChoiceType;
            Data = value;
        }

        public Identification(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            Data = recipientByNameAndAddress;
        }
        
        public object Data { get; internal set; }

    }
}
