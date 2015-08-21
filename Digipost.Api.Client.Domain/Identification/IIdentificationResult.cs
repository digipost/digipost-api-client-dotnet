using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public interface IIdentificationResult
    {
        IdentificationResultType ResultType { get;}

        IdentificationError? Error { get; }

        string Data { get; set; }

    }
}