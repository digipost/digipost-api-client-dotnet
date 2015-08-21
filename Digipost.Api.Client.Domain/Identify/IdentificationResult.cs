using System;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identify
{
    public class IdentificationResult : IIdentificationResult
    {
        public IdentificationResult(IdentificationResultType resultType, string result)
        {
            ResultType = resultType;
            SetResultByIdentificationResultType(result);
        }

        public IdentificationResultType ResultType { get; private set; }

        public IdentificationError? Error { get; private set; }

        public string Data { get; set; }

        private static IdentificationError ParseToIdentificationError(string identificationError)
        {
            return (IdentificationError) Enum.Parse(typeof (IdentificationError), identificationError);
        }

        private void SetResultByIdentificationResultType(string result)
        {
            bool allSuccessfulResultType = ResultType == IdentificationResultType.DigipostAddress ||
                                           ResultType == IdentificationResultType.Personalias;

            if (allSuccessfulResultType)
            {
                Data = result;
            }
            else
            {
                Error = ParseToIdentificationError(result);
            }
        }
    }
}
