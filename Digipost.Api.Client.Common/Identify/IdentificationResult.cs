using System;
using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Identify
{
    public class IdentificationResult : IIdentificationResult
    {
        internal IdentificationResult(IdentificationResultType resultType, IdentificationError identificationError)
        {
            ResultType = resultType;
            Error = identificationError;
        }

        internal IdentificationResult(IdentificationResultType resultType, string data)
        {
            ResultType = resultType;
            Data = data;

#pragma warning disable CS0618
            var allSuccessfulResultType = ResultType == IdentificationResultType.DigipostAddress ||
                                          ResultType == IdentificationResultType.Personalias;
#pragma warning restore CS0618
            if (!allSuccessfulResultType) throw new ArgumentException("Do not use this constructor for other than positive identification ");
        }

        public IdentificationResultType ResultType { get; }

        public IdentificationError? Error { get; }

        public string Data { get; set; }
    }
}
