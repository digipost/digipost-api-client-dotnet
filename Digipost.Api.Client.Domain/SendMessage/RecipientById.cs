using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientById : DigipostRecipient
    {
        public RecipientById(IdentificationType identificationType, string id)
        {
            IdentificationType = identificationType;
            Id = id;
        }

        public string Id { get; set; }

        public IdentificationType IdentificationType { get; set; }
    }
}
