using System;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identification;

namespace Digipost.Api.Client.Domain.Utilities
{
    internal class DtoConverter
    {
        public static IdentificationDto ToDataTransferObject(Identification.Identification identification)
        {
            if (identification.IdentificationChoice == IdentificationChoice.NameAndAddress)
            {
                return new IdentificationDto((RecipientByNameAndAddress) identification.Data);
            }
            
            return new IdentificationDto(identification.IdentificationChoice, identification.Data.ToString());
        }

        public static IdentificationResult FromDataTransferObject(IdentificationResultDto identificationResultDto)
        {
            if(identificationResultDto.IdentificationValue == null)
                return new IdentificationResult(identificationResultDto.IdentificationResultType, "");
                
            return new IdentificationResult(identificationResultDto.IdentificationResultType, identificationResultDto.IdentificationValue.ToString());
        }
    }
}
