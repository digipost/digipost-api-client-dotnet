using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.Utilities
{
    internal class DataTransferObjectConverter
    {
        public static IdentificationDataTransferObject ToDataTransferObject(IIdentification identification)
        {
            IdentificationDataTransferObject identificationDataTransferObject = null;

            if (identification is Identification)
            {
                identificationDataTransferObject = IdentificationDataTransferObjectFromIdentification((Identification) identification);
            }

            if (identification is IdentificationById)
            {
                identificationDataTransferObject = IdentificationDataTransferObjectFromIdentificationById((IdentificationById) identification);
            }

            if (identification is IdentificationByNameAndAddress)
            {
                identificationDataTransferObject = IdentificationDataTranferObjectFromIdentificationByNameAndAddress((IdentificationByNameAndAddress) identification);
            }

            return identificationDataTransferObject;
        }

        private static IdentificationDataTransferObject IdentificationDataTransferObjectFromIdentification(
            Identification identification)
        {
            IdentificationDataTransferObject identificationDataTransferObject = null;

            if (identification.IdentificationChoiceType == IdentificationChoiceType.NameAndAddress)
            {
                identificationDataTransferObject =
                    new IdentificationDataTransferObject((RecipientByNameAndAddress) identification.Data);
            }
            else
            {
                identificationDataTransferObject = new IdentificationDataTransferObject(identification.IdentificationChoiceType, identification.Data.ToString());
            }

            return identificationDataTransferObject;
        }

        private static IdentificationDataTransferObject IdentificationDataTransferObjectFromIdentificationById(IdentificationById identificationById)
        {
            return new  IdentificationDataTransferObject(
                identificationById.IdentificationChoiceType, 
                identificationById.Value
                );
        }

        private static IdentificationDataTransferObject IdentificationDataTranferObjectFromIdentificationByNameAndAddress(
            IdentificationByNameAndAddress identificationByNameAndAddress)
        {
            return new IdentificationDataTransferObject(identificationByNameAndAddress.RecipientByNameAndAddress);
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
                document.ContentBytes, document.AuthenticationLevel, document.SensitivityLevel, ToDataTransferObject(document.SmsNotification))
            {
                Guid = document.Guid
            };

            return documentDataTransferObject;
        }

        public static RecipientDataTransferObject ToDataTransferObject(IRecipient recipient)
        {
            PrintDetailsDataTransferObject printDetailsDataTransferObject =
                ToDataTransferObject(recipient.PrintDetails);

            RecipientDataTransferObject recipientDataTransferObject;
            switch (recipient.IdentificationType)
            {
                case IdentificationChoiceType.NameAndAddress:
                    recipientDataTransferObject = new RecipientDataTransferObject((RecipientByNameAndAddress)recipient.IdentificationValue, printDetailsDataTransferObject);
                    break;
                default:
                    IdentificationChoiceType identificationType = (IdentificationChoiceType)recipient.IdentificationType;

                    recipientDataTransferObject = new RecipientDataTransferObject(identificationType, (string)recipient.IdentificationValue, printDetailsDataTransferObject);
                    break;
                case null:
                    recipientDataTransferObject = new RecipientDataTransferObject(printDetailsDataTransferObject);
                    break;
            }

            return recipientDataTransferObject;
        }

        public static PrintDetailsDataTransferObject ToDataTransferObject(IPrintDetails printDetails)
        {
            if (printDetails == null)
                return null;

            var recipient = printDetails.PrintRecipient;
            var ret = printDetails.PrintReturnRecipient;

            PrintDetailsDataTransferObject printDetailsDataTransferObject = new PrintDetailsDataTransferObject(null, null, printDetails.PostType, printDetails.PrintColors, printDetails.NondeliverableHandling);
            
            //Set recipient on pDTO
            if (recipient.Address is INorwegianAddress)
            {
                var addr = (INorwegianAddress) recipient.Address;
                printDetailsDataTransferObject.PrintRecipientDataTransferObject = new PrintRecipientDataTransferObject(recipient.Name,
                    new NorwegianAddressDataTransferObject(addr.PostalCode, addr.City, addr.AddressLine1,
                        addr.AddressLine2, addr.AddressLine3));
            }
            else
            {
                var addr = (IForeignAddress)recipient.Address;
                printDetailsDataTransferObject.PrintRecipientDataTransferObject = new PrintRecipientDataTransferObject(recipient.Name,
                    new ForeignAddressDataTransferObject(addr.CountryIdentifier, addr.CountryIdentifierValue, addr.AddressLine1, addr.AddressLine2, addr.AddressLine3, addr.Addressline4));
            }

            {
                //Set return recipient on pDTO
                if (ret.Address is INorwegianAddress)
                {
                    var addr = (INorwegianAddress) ret.Address;
                    printDetailsDataTransferObject.PrintReturnRecipientDataTransferObject = new PrintReturnRecipientDataTransferObject(recipient.Name,
                        new NorwegianAddressDataTransferObject(addr.PostalCode, addr.City, addr.AddressLine1,
                            addr.AddressLine2, addr.AddressLine3));
                }
                else
                {
                    var addr = (IForeignAddress) ret.Address;
                    printDetailsDataTransferObject.PrintRecipientDataTransferObject = new PrintRecipientDataTransferObject(recipient.Name,
                        new ForeignAddressDataTransferObject(addr.CountryIdentifier, addr.CountryIdentifierValue,
                            addr.AddressLine1, addr.AddressLine2, addr.AddressLine3, addr.Addressline4));
                }
            }

            return printDetailsDataTransferObject;
        }

        public static ForeignAddressDataTransferObject ToDataTransferObject(IForeignAddress foreignAddress)
        {
            return new ForeignAddressDataTransferObject(
                foreignAddress.CountryIdentifier, 
                foreignAddress.CountryIdentifierValue, 
                foreignAddress.AddressLine1, 
                foreignAddress.AddressLine2, 
                foreignAddress.AddressLine3, 
                foreignAddress.Addressline4
                );
        }

        public static NorwegianAddressDataTransferObject ToDataTransferObject(INorwegianAddress norwegianAddress)
        {
            return new NorwegianAddressDataTransferObject(norwegianAddress.PostalCode, norwegianAddress.City,norwegianAddress.AddressLine1,norwegianAddress.AddressLine2, norwegianAddress.AddressLine3);
        }

        public static PrintRecipientDataTransferObject ToDataTransferObject(PrintRecipient printRecipient)
        {
            PrintRecipientDataTransferObject printDataTransferObject;

            var addressType = printRecipient.Address.GetType();
            if (typeof (INorwegianAddress).IsAssignableFrom(addressType))
            {
                var address = printRecipient.Address as NorwegianAddress;

                printDataTransferObject = new PrintRecipientDataTransferObject(printRecipient.Name,
                    new NorwegianAddressDataTransferObject(address.PostalCode, address.City, address.AddressLine1, address.AddressLine2, address.AddressLine3));
            }
            else
            {
                var address = printRecipient.Address as ForeignAddress;

                printDataTransferObject = new PrintRecipientDataTransferObject(printRecipient.Name, 
                    new ForeignAddressDataTransferObject(address.CountryIdentifier, address.CountryIdentifierValue, address.AddressLine1, address.AddressLine2, address.AddressLine3, address.Addressline4));
            }

            return printDataTransferObject;

        }

        public static PrintReturnRecipientDataTransferObject ToDataTransferObject(PrintReturnRecipient printRecipient)
        {
            PrintReturnRecipientDataTransferObject printDataTransferObject;

            var addressType = printRecipient.Address.GetType();
            if (typeof(INorwegianAddress).IsAssignableFrom(addressType))
            {
                var address = printRecipient.Address as NorwegianAddress;

                printDataTransferObject = new PrintReturnRecipientDataTransferObject(printRecipient.Name,
                    new NorwegianAddressDataTransferObject(address.PostalCode, address.City, address.AddressLine1, address.AddressLine2, address.AddressLine3));
            }
            else
            {
                var address = printRecipient.Address as ForeignAddress;

                printDataTransferObject = new PrintReturnRecipientDataTransferObject(printRecipient.Name,
                    new ForeignAddressDataTransferObject(address.CountryIdentifier, address.CountryIdentifierValue, address.AddressLine1, address.AddressLine2, address.AddressLine3, address.Addressline4));
            }

            return printDataTransferObject;

        }

        public static SmsNotificationDataTransferObject ToDataTransferObject(ISmsNotification smsNotification)
        {
            if (smsNotification == null)
                return null;

            var timesAsListedTimes = smsNotification.NotifyAtTimes.Select(dateTime => new ListedTimeDataTransferObject(dateTime)).ToList();

            var smsNotificationDataTransferObject = new SmsNotificationDataTransferObject()
            {
                NotifyAfterHours = smsNotification.NotifyAfterHours,
                NotifyAtTimes = timesAsListedTimes
            };


            return smsNotificationDataTransferObject;
        }

        public static IIdentificationResult FromDataTransferObject(IdentificationResultDataTransferObject identificationResultDto)
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

        public static IMessageDeliveryResult FromDataTransferObject(MessageDeliveryResultDataTransferObject messageDeliveryResultDataTransferObject)
        {
            IMessageDeliveryResult messageDeliveryResult = new MessageDeliveryResult()
            {
                PrimaryDocument = FromDataTransferObject(messageDeliveryResultDataTransferObject.PrimaryDocumentDataTransferObject), 
                Attachments = messageDeliveryResultDataTransferObject.AttachmentsDataTransferObject.Select(documentDataTransferObject => FromDataTransferObject(documentDataTransferObject)).ToList(),
                DeliveryTime = messageDeliveryResultDataTransferObject.DeliveryTime,
                DeliveryMethod = messageDeliveryResultDataTransferObject.DeliveryMethod,
                Status = messageDeliveryResultDataTransferObject.Status
            };

            return messageDeliveryResult;
        }

        public static IDocument FromDataTransferObject(DocumentDataTransferObject documentDataTransferObject)
        {
            return new Document(documentDataTransferObject.Subject, documentDataTransferObject.FileType, documentDataTransferObject.ContentBytes, documentDataTransferObject.AuthenticationLevel, documentDataTransferObject.SensitivityLevel, FromDataTransferObject(documentDataTransferObject.SmsNotification))
            {
                Guid = documentDataTransferObject.Guid
            };
        }

        public static ISmsNotification FromDataTransferObject(SmsNotificationDataTransferObject smsNotificationDataTransferObject)
        {
            if (smsNotificationDataTransferObject == null)
                return null;

            var dateTimes = smsNotificationDataTransferObject.NotifyAtTimes
                .Select(listedTime => listedTime.Time)
                .ToList();
            
            var smsNotification = new SmsNotification()
            {
                NotifyAfterHours = smsNotificationDataTransferObject.NotifyAfterHours, 
                NotifyAtTimes = dateTimes
            };

            return smsNotification;
        }
    }
}
