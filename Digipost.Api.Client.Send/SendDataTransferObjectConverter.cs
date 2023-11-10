using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using V8 = Digipost.Api.Client.Common.Generated.V8;

namespace Digipost.Api.Client.Send
{
    internal static class SendDataTransferObjectConverter
    {
        internal static V8.Message ToDataTransferObject(this IMessage message)
        {
            var primaryDocumentDataTransferObject = ToDataTransferObject(message.PrimaryDocument);

            var messageDto = new V8.Message
            {
                SenderId = message.Sender.Id,
                SenderIdSpecified = true,
                PrimaryDocument = primaryDocumentDataTransferObject,
                MessageId = message.Id
            };

            if (message is PrintMessage)
            {
                messageDto.Recipient = new V8.MessageRecipient()
                {
                    PrintDetails = message.PrintDetails.ToDataTransferObject()
                };
            }
            else
            {
                messageDto.Recipient = message.DigipostRecipient.ToDataTransferObject();
                messageDto.Recipient.PrintDetails = message.PrintDetails.ToDataTransferObject();
            }

            foreach (var document in message.Attachments.Select(ToDataTransferObject))
            {
                messageDto.Attachment.Add(document);
            }

            if (message.DeliveryTimeSpecified)
            {
                messageDto.DeliveryTime = message.DeliveryTime.Value;
                messageDto.DeliveryTimeSpecified = true;
            }

            if (message.PrintIfUnreadAfterSpecified)
            {
                messageDto.PrintIfUnread = message.PrintIfUnread.ToDataTransferObject();
            }

            if (message.RequestForRegistrationSpecified)
            {
                messageDto.RequestForRegistration = message.RequestForRegistration.ToDataTransferObject();
            }

            return messageDto;
        }

        internal static V8.Document ToDataTransferObject(this IDocument document)
        {
            var documentDto = new V8.Document
            {
                Subject = document.Subject,
                FileType = document.FileType,
                AuthenticationLevel = document.AuthenticationLevel.ToAuthenticationLevel(),
                AuthenticationLevelSpecified = true,
                SensitivityLevel = document.SensitivityLevel.ToSensitivityLevel(),
                SensitivityLevelSpecified = true,
                SmsNotification = ToDataTransferObject(document.SmsNotification),
                Uuid = document.Guid
            };

            if (document.DataType != null)
            {
                documentDto.DataType = new V8.DataType()
                {
                    Any = document.DataType.ToXmlDocument().DocumentElement
                };
            }

            return documentDto;
        }

        internal static V8.SmsNotification ToDataTransferObject(this ISmsNotification smsNotification)
        {
            if (smsNotification == null)
                return null;

            var smsNotificationDto = new V8.SmsNotification();

            if (smsNotification.NotifyAtTimes.Count > 0)
            {
                var timesAsListedTimes = smsNotification.NotifyAtTimes.Select(dateTime => new V8.ListedTime {Time = dateTime, TimeSpecified = true});
                foreach (var timesAsListedTime in timesAsListedTimes)
                {
                    smsNotificationDto.At.Add(timesAsListedTime);
                }
            }

            if (smsNotification.NotifyAfterHours.Count > 0)
            {
                foreach (var i in smsNotification.NotifyAfterHours.ToArray())
                {
                    smsNotificationDto.AfterHours.Add(i);
                }
            }

            return smsNotificationDto;
        }

        internal static IMessageDeliveryResult FromDataTransferObject(this V8.MessageDelivery messageDeliveryDto)
        {
            IMessageDeliveryResult messageDeliveryResult = new MessageDeliveryResult
            {
                MessageId = messageDeliveryDto.MessageId,
                PrimaryDocument = FromDataTransferObject(messageDeliveryDto.PrimaryDocument),
                Attachments = messageDeliveryDto.Attachment?.Select(FromDataTransferObject).ToList(),
                DeliveryTime = messageDeliveryDto.DeliveryTime,
                DeliveryMethod = messageDeliveryDto.DeliveryMethod.ToDeliveryMethod(),
                Status = messageDeliveryDto.Status.ToMessageStatus(),
                SenderId = messageDeliveryDto.SenderId
            };

            return messageDeliveryResult;
        }

        internal static IDocument FromDataTransferObject(this V8.Document documentDto)
        {
            return new Document(documentDto.Subject, documentDto.FileType, documentDto.AuthenticationLevel.ToAuthenticationLevel(), documentDto.SensitivityLevel.ToSensitivityLevel(), FromDataTransferObject(documentDto.SmsNotification))
            {
                Guid = documentDto.Uuid,
                ContentHash = new ContentHash {HashAlgoritm = documentDto.ContentHash.HashAlgorithm, Value = documentDto.ContentHash.Value},
                Links = documentDto.Link.FromDataTransferObject()
            };
        }

        internal static ISmsNotification FromDataTransferObject(this V8.SmsNotification smsNotificationDto)
        {
            if (smsNotificationDto == null)
                return null;

            var smsNotification = new SmsNotification
            {
                NotifyAfterHours = smsNotificationDto.AfterHours?.ToList() ?? new List<int>(),
                NotifyAtTimes = smsNotificationDto.At?.Select(listedTime => listedTime.Time).ToList() ?? new List<DateTime>()
            };

            return smsNotification;
        }

        internal static DocumentStatus FromDataTransferObject(this V8.DocumentStatus dto)
        {
            return new DocumentStatus(
                dto.Uuid,
                dto.SenderId,
                dto.Created,
                dto.Status.ToDeliveryStatus(),
                dto.ReadSpecified ? dto.Read.ToRead() : (DocumentStatus.Read?) null,
                dto.Channel.ToDeliveryMethod(),
                dto.ContentHash,
                dto.DeliveredSpecified ? dto.Delivered : (DateTime?) null,
                dto.IsPrimaryDocumentSpecified ? dto.IsPrimaryDocument : (bool?) null,
                dto.ContentHashAlgorithmSpecified ? dto.ContentHashAlgorithm.ToHashAlgoritm() : (HashAlgoritm?) null
            );
        }

        private static DocumentStatus.DocumentDeliveryStatus ToDeliveryStatus(this V8.DeliveryStatus deliveryStatus)
        {
            switch (deliveryStatus)
            {
                case V8.DeliveryStatus.Delivered:
                    return DocumentStatus.DocumentDeliveryStatus.DELIVERED;
                case V8.DeliveryStatus.NotDelivered:
                    return DocumentStatus.DocumentDeliveryStatus.NOT_DELIVERED;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static DocumentStatus.Read ToRead(this V8.Read read)
        {
            switch (read)
            {
                case V8.Read.Y:
                    return DocumentStatus.Read.YES;
                case V8.Read.N:
                    return DocumentStatus.Read.NO;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static V8.AdditionalData ToDataTransferObject(this IAdditionalData additionalData)
        {
            var dto = new V8.AdditionalData
            {
                SenderId = additionalData.Sender.Id,
                SenderIdSpecified = true,
                DataType = new V8.DataType()
                {
                    Any = additionalData.DataType.ToXmlDocument().DocumentElement
                }
            };

            return dto;
        }
    }
}
