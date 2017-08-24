using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Recipient;

namespace Digipost.Api.Client.Send
{
    internal class SendDataTransferObjectConverter
    {
        public static message ToDataTransferObject(IMessage message)
        {
            var primaryDocumentDataTransferObject = ToDataTransferObject(message.PrimaryDocument);

            var messageDto = new message
            {
                Item = message.Sender.Id,
                primarydocument = primaryDocumentDataTransferObject,
                messageid = message.Id,
                recipient = DataTransferObjectConverter.ToDataTransferObject(message.DigipostRecipient)
            };

            messageDto.recipient.printdetails = DataTransferObjectConverter.ToDataTransferObject(message.PrintDetails);

            if (message.DigipostRecipient is RecipientById)
            {
                var identificationType = ((RecipientById) message.DigipostRecipient).IdentificationType;
                messageDto.recipient.ItemElementName = identificationType.ToItemChoiceType1();
            }
            else if (message.DigipostRecipient is RecipientByNameAndAddress)
            {
                messageDto.recipient.ItemElementName = ItemChoiceType1.nameandaddress;
            }

            messageDto.attachment = message.Attachments.Select(ToDataTransferObject).ToArray();

            if (message.DeliveryTimeSpecified)
            {
                messageDto.deliverytime = message.DeliveryTime.Value;
                messageDto.deliverytimeSpecified = true;
            }

            return messageDto;
        }

        public static document ToDataTransferObject(IDocument document)
        {
            document documentDto;

            if (document is Invoice)
            {
                var invoice = (Invoice) document;

                var invoiceDto = new invoice
                {
                    amount = invoice.Amount,
                    duedate = invoice.Duedate,
                    kid = invoice.Kid,
                    account = invoice.Account
                };

                documentDto = invoiceDto;
            }
            else
            {
                documentDto = new document();
            }

            documentDto.subject = document.Subject;
            documentDto.filetype = document.FileType;
            documentDto.authenticationlevel = document.AuthenticationLevel.ToAuthenticationLevel();
            documentDto.authenticationlevelSpecified = true;
            documentDto.sensitivitylevel = document.SensitivityLevel.ToSensitivityLevel();
            documentDto.sensitivitylevelSpecified = true;
            documentDto.smsnotification = DataTransferObjectConverter.ToDataTransferObject(document.SmsNotification);
            documentDto.uuid = document.Guid;

            return documentDto;
        }

        public static IMessageDeliveryResult FromDataTransferObject(messagedelivery messageDeliveryDto)
        {
            IMessageDeliveryResult messageDeliveryResult = new MessageDeliveryResult
            {
                MessageId = messageDeliveryDto.messageid,
                PrimaryDocument = FromDataTransferObject(messageDeliveryDto.primarydocument),
                Attachments = messageDeliveryDto.attachment?.Select(FromDataTransferObject).ToList(),
                DeliveryTime = messageDeliveryDto.deliverytime,
                DeliveryMethod = messageDeliveryDto.deliverymethod.ToDeliveryMethod(),
                Status = messageDeliveryDto.status.ToMessageStatus(),
                SenderId = messageDeliveryDto.senderid
            };

            return messageDeliveryResult;
        }

        public static IDocument FromDataTransferObject(document documentDto)
        {
            return new Document(documentDto.subject, documentDto.filetype, documentDto.authenticationlevel.ToAuthenticationLevel(), documentDto.sensitivitylevel.ToSensitivityLevel(), FromDataTransferObject(documentDto.smsnotification))
            {
                Guid = documentDto.uuid,
                ContentHash = new ContentHash {HashAlgoritm = documentDto.contenthash.hashalgorithm, Value = documentDto.contenthash.Value}
            };
        }

        public static ISmsNotification FromDataTransferObject(smsnotification smsNotificationDto)
        {
            if (smsNotificationDto == null)
                return null;

            var smsNotification = new SmsNotification
            {
                NotifyAfterHours = smsNotificationDto.afterhours?.ToList() ?? new List<int>(),
                NotifyAtTimes = smsNotificationDto.at?.Select(listedTime => listedTime.time).ToList() ?? new List<DateTime>()
            };

            return smsNotification;
        }
    }
}