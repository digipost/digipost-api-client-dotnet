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
            //Hvis det er en ok, sett resultat
            if (IdentificationResultType == IdentificationResultType.Digipostaddress ||
                IdentificationResultType == IdentificationResultType.Personalias)
            {
                Result = result;
            }

            //Hvis det er en feilenum, sett 
        }

        public IdentificationResultType IdentificationResultType { get; private set; }
        
        public IdentificationError IdentificationError { get; private set; }

        public string Result { get; set; }

        private IdentificationError ParseToIdentificationError(string identificationError)
        {
            throw new NotImplementedException();
        }

        //Fjern
        public IdentificationResultCode IdentificationResultCode { get; set; }

        //Fjern
        public object IdentificationValue { get; set; }

    }
}
