using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.Identify
{
    public class IdentificationByNameAndAddress : IIdentification
    {
        public IdentificationByNameAndAddress(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            RecipientByNameAndAddress = recipientByNameAndAddress;
        }

        public RecipientByNameAndAddress RecipientByNameAndAddress { get; set; }

        public IdentificationChoiceType IdentificationChoiceType
        {
            get { return IdentificationChoiceType.NameAndAddress; }
        }

        public object Data
        {
            get { return RecipientByNameAndAddress; }
        }
    }
}
