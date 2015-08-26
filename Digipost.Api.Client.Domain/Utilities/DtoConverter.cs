using System;
using Digipost.Api.Client.Domain.DataTransferObject;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.Utilities
{
    internal class DtoConverter
    {
        public static IdentificationDataTransferObject ToDataTransferObject(IIdentification identification)
        {
            if (identification.IdentificationChoiceType == IdentificationChoiceType.NameAndAddress)
            {
                return new IdentificationDataTransferObject((RecipientByNameAndAddress)identification.Data);

            }

            return new IdentificationDataTransferObject(identification.IdentificationChoiceType, identification.Data.ToString());
        }

        public static MessageDataTransferObject ToDataTransferObject(IMessage message)
        {
            RecipientDataTransferObject recipient = ToDataTransferObject(message.Recipient);
            DocumentDataTransferObject primaryDocumentDataTransferObject = ToDataTransferObject(message.PrimaryDocument);

            var messageDataTransferObject = new MessageDataTransferObject(recipient, primaryDocumentDataTransferObject, message.SenderId);

            foreach (var attachment in message.Attachments)
            {
                var attachmentDataTransferObject = ToDataTransferObject(attachment);
                messageDataTransferObject.Attachments.Add(attachmentDataTransferObject);
            }

            messageDataTransferObject.DeliveryTime = message.DeliveryTime;

            return messageDataTransferObject;
        }

        public static DocumentDataTransferObject ToDataTransferObject(IDocument document)
        {
            var documentDataTransferObject = new DocumentDataTransferObject(document.Subject, document.FileType,
                document.ContentBytes, document.AuthenticationLevel, document.SensitivityLevel, document.SmsNotification)
            {
                Guid = document.Guid
            };

            return documentDataTransferObject;
        }

        public static RecipientDataTransferObject ToDataTransferObject(IRecipient recipient)
        {
            RecipientDataTransferObject recipientDataTransferObject;
            switch (recipient.IdentificationType)
            {
                case IdentificationChoiceType.NameAndAddress:
                    recipientDataTransferObject = new RecipientDataTransferObject((RecipientByNameAndAddress)recipient.IdentificationValue, recipient.PrintDetails);
                    break;
                default:
                    IdentificationChoiceType identificationType = (IdentificationChoiceType)recipient.IdentificationType;

                    recipientDataTransferObject = new RecipientDataTransferObject(identificationType, (string)recipient.IdentificationValue, recipient.PrintDetails);
                    break;
                case null:
                    recipientDataTransferObject = new RecipientDataTransferObject(recipient.PrintDetails);
                    break;
            }

            return recipientDataTransferObject;
        }


        public static IdentificationResult FromDataTransferObject(IdentificationResultDataTransferObject identificationResultDto)
        {
            IdentificationResult identificationResult;

            if (identificationResultDto.IdentificationValue == null)
            {
                identificationResult = new IdentificationResult(identificationResultDto.IdentificationResultType, "");
            }
            else
            {
                identificationResult = new IdentificationResult(identificationResultDto.IdentificationResultType, identificationResultDto.IdentificationValue.ToString());
            }

            return identificationResult;
        }


    }
}
