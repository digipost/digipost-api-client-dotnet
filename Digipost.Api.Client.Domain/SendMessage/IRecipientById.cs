using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IRecipientById
    {
        IdentificationType IdentificationType { get; set; }

        string Id { get; set; }
    }
}