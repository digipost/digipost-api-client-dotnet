using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Recipient
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

        public override string ToString()
        {
            return $"Id: {Id}, IdentificationType: {IdentificationType}";
        }
    }
}