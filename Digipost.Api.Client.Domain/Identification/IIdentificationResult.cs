using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public interface IIdentificationResult
    {
        IdentificationResultCode IdentificationResultCode { get; }

        object IdentificationValue { get; set; }

        IdentificationResultType IdentificationResultType { get;}
    }
}