using System;
using Digipost.Api.Client.Domain.DataTransferObject;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;

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

        public static MessageDataTransferObject ToDataTransferObject(Message message)
        {
            RecipientDataTransferObject recipient = ToDataTransferObject(message.Recipient);
            DocumentDataTransferObject primaryDocumentDataTransferObject = ToDataTransferObject(message.PrimaryDocument);
            
            return new MessageDataTransferObject(recipient, primaryDocumentDataTransferObject, message.SenderId);
        }

        public static DocumentDataTransferObject ToDataTransferObject(IDocument document)
        {
            var documentDataTransferObject = new DocumentDataTransferObject(document.Subject, document.FileType, document.ContentBytes, document.AuthenticationLevel, document.SensitivityLevel, document.SmsNotification);
            documentDataTransferObject.Guid = document.Guid;

            return documentDataTransferObject;
        }

        public static RecipientDataTransferObject ToDataTransferObject(IRecipient recipient)
        {
            if (recipient.IdentificationType == IdentificationChoiceType.NameAndAddress)
            {
                return new RecipientDataTransferObject((RecipientByNameAndAddress)recipient.IdentificationValue, recipient.PrintDetails);
            }

            if (recipient.IdentificationType == null)
            {
                return new RecipientDataTransferObject(recipient.PrintDetails);
            }

            IdentificationChoiceType identificationType = (IdentificationChoiceType) recipient.IdentificationType;
            
            return new RecipientDataTransferObject(identificationType, (string) recipient.IdentificationValue, recipient.PrintDetails);

        }
        

        public static IdentificationResult FromDataTransferObject(IdentificationResultDataTransferObject identificationResultDto)
        {
            if(identificationResultDto.IdentificationValue == null)
                return new IdentificationResult(identificationResultDto.IdentificationResultType, "");
                
            return new IdentificationResult(identificationResultDto.IdentificationResultType, identificationResultDto.IdentificationValue.ToString());
        }

        
    }
}
