using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public interface IIdentification
    {
        object IdentificationValue { get; set; }

        IdentificationChoice IdentificationType { get; set; }
    }
}