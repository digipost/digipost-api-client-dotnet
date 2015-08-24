using System;
using Digipost.Api.Client.Domain.DataTransferObject;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;

namespace Digipost.Api.Client.Domain.Utilities
{
    internal class DtoConverter
    {
        public static IdentificationDataTransferObject ToDataTransferObject(Identification identification)
        {
            if (identification.IdentificationChoiceType == IdentificationChoiceType.NameAndAddress)
            {
                return new IdentificationDataTransferObject((RecipientByNameAndAddress) identification.Data);
            }
            
            return new IdentificationDataTransferObject(identification.IdentificationChoiceType, identification.Data.ToString());
        }

        

        public static IdentificationResult FromDataTransferObject(IdentificationResultDataTransferObject identificationResultDto)
        {
            if(identificationResultDto.IdentificationValue == null)
                return new IdentificationResult(identificationResultDto.IdentificationResultType, "");
                
            return new IdentificationResult(identificationResultDto.IdentificationResultType, identificationResultDto.IdentificationValue.ToString());
        }

        
    }
}
