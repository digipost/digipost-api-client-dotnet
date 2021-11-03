using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Extensions;
using V7;

namespace Digipost.Api.Client.Send
{
    internal class SendDataTransferObjectConverter
    {
        public static V7.Message ToDataTransferObject(IMessage message)
        {
            var primaryDocumentDataTransferObject = ToDataTransferObject(message.PrimaryDocument);

            var messageDto = new V7.Message
            {
                Sender_Id = message.Sender.Id,
                Primary_Document = primaryDocumentDataTransferObject,
                Message_Id = message.Id,
                Recipient = DataTransferObjectConverter.ToDataTransferObject(message.DigipostRecipient)
            };

            messageDto.Recipient.Print_Details = DataTransferObjectConverter.ToDataTransferObject(message.PrintDetails);


            foreach (var document in message.Attachments.Select(ToDataTransferObject))
            {
                messageDto.Attachment.Add(document);
            }

            if (message.DeliveryTimeSpecified)
            {
                messageDto.Delivery_Time = message.DeliveryTime.Value;
                messageDto.Delivery_TimeSpecified = true;
            }

            if (message.PrintIfUnreadAfterSpecified)
            {
                messageDto.Print_If_Unread = DataTransferObjectConverter.ToDataTransferObject(message.PrintIfUnread);
            }

            return messageDto;
        }

        public static V7.Document ToDataTransferObject(IDocument document)
        {
            V7.Document documentDto;

            if (document is Invoice)
            {
                var invoice = (Invoice) document;

                var invoiceDto = new V7.Invoice
                {
                    Amount = invoice.Amount,
                    Due_Date = invoice.Duedate,
                    Kid = invoice.Kid,
                    Account = invoice.Account
                };

                documentDto = invoiceDto;
            }
            else
            {
                documentDto = new V7.Document();
            }

            documentDto.Subject = document.Subject;
            documentDto.File_Type = document.FileType;
            documentDto.Authentication_Level = document.AuthenticationLevel.ToAuthenticationLevel();
            documentDto.Authentication_LevelSpecified = true;
            documentDto.Sensitivity_Level = document.SensitivityLevel.ToSensitivityLevel();
            documentDto.Sensitivity_LevelSpecified = true;
            documentDto.Sms_Notification = DataTransferObjectConverter.ToDataTransferObject(document.SmsNotification);
            documentDto.Uuid = document.Guid;

            if (document.DataType != null)
            {
                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(document.DataType);
                documentDto.Data_Type = new Data_Type()
                {
                    Any = xmldoc.DocumentElement
                };
            }

            return documentDto;
        }

        public static IMessageDeliveryResult FromDataTransferObject(V7.Message_Delivery messageDeliveryDto)
        {
            IMessageDeliveryResult messageDeliveryResult = new MessageDeliveryResult
            {
                MessageId = messageDeliveryDto.Message_Id,
                PrimaryDocument = FromDataTransferObject(messageDeliveryDto.Primary_Document),
                Attachments = messageDeliveryDto.Attachment?.Select(FromDataTransferObject).ToList(),
                DeliveryTime = messageDeliveryDto.Delivery_Time,
                DeliveryMethod = messageDeliveryDto.Delivery_Method.ToDeliveryMethod(),
                Status = messageDeliveryDto.Status.ToMessageStatus(),
                SenderId = messageDeliveryDto.Sender_Id
            };

            return messageDeliveryResult;
        }

        public static IDocument FromDataTransferObject(V7.Document documentDto)
        {
            return new Document(documentDto.Subject, documentDto.File_Type, documentDto.Authentication_Level.ToAuthenticationLevel(), documentDto.Sensitivity_Level.ToSensitivityLevel(), FromDataTransferObject(documentDto.Sms_Notification))
            {
                Guid = documentDto.Uuid,
                ContentHash = new ContentHash {HashAlgoritm = documentDto.Content_Hash.Hash_Algorithm, Value = documentDto.Content_Hash.Value}
            };
        }

        public static ISmsNotification FromDataTransferObject(V7.Sms_Notification smsNotificationDto)
        {
            if (smsNotificationDto == null)
                return null;

            var smsNotification = new SmsNotification
            {
                NotifyAfterHours = smsNotificationDto.After_Hours?.ToList() ?? new List<int>(),
                NotifyAtTimes = smsNotificationDto.At?.Select(listedTime => listedTime.Time).ToList() ?? new List<DateTime>()
            };

            return smsNotification;
        }
    }
}
