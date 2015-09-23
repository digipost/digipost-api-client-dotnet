using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class RecipientById : DigipostRecipient
    {
        public RecipientById(IdentificationType identificationType, string id, IPrintDetails printDetails = null)
        {
            IdentificationType = identificationType;
            Id = id;
            PrintDetails = printDetails;
        }

        public string Id { get; set; }

        public IPrintDetails PrintDetails { get; set; }

        public IdentificationType IdentificationType { get; set; }
    }
}
