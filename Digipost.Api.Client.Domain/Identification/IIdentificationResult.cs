using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public interface IIdentificationResult
    {
        IdentificationResultCode IdentificationResultCode { get; set; }

        object IdentificationValue { get; set; }

        IdentificationResultType IdentificationType { get; set; }
    }
}