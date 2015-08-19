using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public class Identification : IIdentification
    {
        internal readonly IdentificationDto IdentificationDto;

        public object IdentificationValue
        {
            get { return IdentificationDto.IdentificationValue; }
        }

        public IdentificationChoice IdentificationType
        {
            get { return IdentificationDto.IdentificationType; }
        }

        public Identification(IdentificationChoice identificationChoice, string value)
        {
            IdentificationDto = new IdentificationDto(identificationChoice, value);
        }

        public Identification(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            IdentificationDto = new IdentificationDto(recipientByNameAndAddress);
        }

        public object DataTransferObject
        {
            get { return IdentificationDto; }
        }
    }
}
