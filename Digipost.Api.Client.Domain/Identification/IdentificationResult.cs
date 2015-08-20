using System;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public class IdentificationResult : IIdentificationResult
    {
        public IdentificationResult(IdentificationResultType identificationResultType, string result)
        {
            IdentificationResultType = identificationResultType;
            SetResultByIdentificationResultType(result);
        }

        private void SetResultByIdentificationResultType(string result)
        {
            bool allSuccessfulResultType = IdentificationResultType == IdentificationResultType.Digipostaddress ||
                                           IdentificationResultType == IdentificationResultType.Personalias;

            if (allSuccessfulResultType)
            {
                Data = result;
            }
            else
            {
                Error = ParseToIdentificationError(result);
            }
        }

        public IdentificationResultType IdentificationResultType { get; private set; }
        
        public IdentificationError? Error { get; private set; }

        public string Data { get; set; }

        private static IdentificationError ParseToIdentificationError(string identificationError)
        {
            return (IdentificationError) Enum.Parse(typeof (IdentificationError), identificationError);
        }
    }
}
