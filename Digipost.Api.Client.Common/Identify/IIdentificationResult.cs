using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Identify
{
    public interface IIdentificationResult
    {
        IdentificationResultType ResultType { get; }

        IdentificationError? Error { get; }

        string Data { get; set; }
    }
}