using System;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.Identify
{
    [Obsolete("Use IdentificationByNameAndAddress or IdentificationById. Will be removed in future version.")]
    public class Identification : IIdentification
    {
        public IdentificationChoiceType IdentificationChoiceType { get; set; }

        public IdentificationType IdentificationType
        {
            get
            {
                throw new NotImplementedException("Use IdentificationByNameAndAddress or IdentificationById class, as this will be removed in future version.");
            }
        }

        public Identification(IdentificationChoiceType identificationChoiceType, string value)
        {
            IdentificationChoiceType = identificationChoiceType;
            Data = value;
        }

        public Identification(RecipientByNameAndAddressDataTranferObject recipientByNameAndAddressDataTranferObject)
        {
            Data = recipientByNameAndAddressDataTranferObject;
        }
        
        public object Data { get; internal set; }
    }
}
