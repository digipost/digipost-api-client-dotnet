using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Recipient
{
    public interface IRecipientById
    {
        IdentificationType IdentificationType { get; set; }

        string Id { get; set; }
    }
}