using System;
using System.Linq;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Extensions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Extensions;

namespace Digipost.Api.Client.Domain.Utilities
{
    internal class DataTransferObjectConverter
    {
        public static identification ToDataTransferObject(IIdentification identification)
        {
            identification identificationDataTransferObject = null;

            if (identification.DigipostRecipient is RecipientById)
            {
                identificationDataTransferObject = IdentificationDataTransferObjectFromIdentificationById((RecipientById)identification.DigipostRecipient);
            }

            if (identification.DigipostRecipient is RecipientByNameAndAddress)
            {
                identificationDataTransferObject = IdentificationDataTranferObjectFromIdentificationByNameAndAddress((RecipientByNameAndAddress)identification.DigipostRecipient);
            }

            return identificationDataTransferObject;
        }

        private static identification IdentificationDataTransferObjectFromIdentificationById(RecipientById recipientById)
        {
            return new identification
            {
                ItemElementName = recipientById.IdentificationType.ToItemChoiceType(),
                includepersonaliasfordigipostuser = true, //TODO: must expose
                Item = recipientById.Id
            };
        }

        private static identification IdentificationDataTranferObjectFromIdentificationByNameAndAddress(
            RecipientByNameAndAddress recipientByNameAndAddress)
        {
            var identification = new identification
            {
                Item = ToDataTransferObject(recipientByNameAndAddress),
                ItemElementName = ItemChoiceType.nameandaddress,
                includepersonaliasfordigipostuser = true //Todo: Should be watwat?
            };

            return identification;
        }

        public static message ToDataTransferObject(IMessage message)
        {
            var recipient = ToDataTransferObject(message.DigipostRecipient);
            var primaryDocumentDataTransferObject = ToDataTransferObject(message.PrimaryDocument);

            var messageDataTransferObject = new message
            {
                Item = message.SenderId,
                primarydocument = primaryDocumentDataTransferObject,
                messageid = "NULL", //Todo: where from?
                recipient = new messagerecipient
                {
                    printdetails = ToDataTransferObject(message.PrintDetails),
                },
            };

            messageDataTransferObject.recipient = ToDataTransferObject(message.DigipostRecipient);

            if (message.DigipostRecipient is RecipientById)
            {
                var identificationType = ((RecipientById)message.DigipostRecipient).IdentificationType;
                messageDataTransferObject.recipient.ItemElementName = identificationType.ToItemChoiceType1();
            }
            else if (message.DigipostRecipient is RecipientByNameAndAddress)
            {
                messageDataTransferObject.recipient.ItemElementName = ItemChoiceType1.nameandaddress;
            }

            messageDataTransferObject.attachment = message.Attachments.Select(ToDataTransferObject).ToArray();

            if (message.DeliveryTimeSpecified)
            {
                messageDataTransferObject.deliverytime = message.DeliveryTime.Value;
                messageDataTransferObject.deliverytimeSpecified = true;
            }

            return messageDataTransferObject;
        }

        public static document ToDataTransferObject(IDocument document)
        {
            var documentDataTransferObject = new document()
            {
                subject = document.Subject,
                filetype = document.FileType,
               /* contenthash = new contenthash { hashalgorithm = "ALRGORITHM", Value = "VALUE" },*/ //Todo: Hash the content bytes and add here
                Item = document.ContentBytes,
                authenticationlevel = document.AuthenticationLevel.ToAuthenticationLevel(),
                authenticationlevelSpecified = true,
                sensitivitylevel = document.SensitivityLevel.ToSensitivityLevel(),
                sensitivitylevelSpecified = true,
                smsnotification = ToDataTransferObject(document.SmsNotification),
                uuid = document.Guid
            };

            if (document is Invoice)
            {
                return AddInvoiceData((Invoice)document, documentDataTransferObject);
            }

            return documentDataTransferObject;
        }

        private static invoice AddInvoiceData(Invoice invoice, document baseDocument)
        {
            var invoiceDto = (invoice)baseDocument;

            invoiceDto.amount = invoice.Amount;
            invoiceDto.duedate = invoice.Duedate;
            invoiceDto.kid = invoice.Kid;
            invoiceDto.account = invoice.Account;

            return invoiceDto;
        }

        public static messagerecipient ToDataTransferObject(IDigipostRecipient recipient)
        {
            messagerecipient messageRecipientDto = null;

            if (recipient is RecipientById)
            {
                messageRecipientDto = RecipientDataTransferObjectFromRecipientById((RecipientById)recipient);
            }

            if (recipient is RecipientByNameAndAddress)
            {
                messageRecipientDto = RecipientDataTransferObjectFromRecipientByNameAndAddress((RecipientByNameAndAddress)recipient);
            }

            return messageRecipientDto;
        }

        private static messagerecipient RecipientDataTransferObjectFromRecipientById(RecipientById recipient)
        {
            return new messagerecipient()
            {
                ItemElementName = recipient.IdentificationType.ToItemChoiceType1(),
                Item = recipient.Id
            };
        }

        private static messagerecipient RecipientDataTransferObjectFromRecipientByNameAndAddress(
            IRecipientByNameAndAddress recipientByNameAndAddress)
        {
            var nameAndAddressDto = new nameandaddress()
            {
                fullname = recipientByNameAndAddress.FullName,
                addressline1 = recipientByNameAndAddress.AddressLine1,
                addressline2 = recipientByNameAndAddress.AddressLine2,
                postalcode = recipientByNameAndAddress.PostalCode,
                city = recipientByNameAndAddress.City,
                emailaddress = recipientByNameAndAddress.Email,
                phonenumber = recipientByNameAndAddress.PhoneNumber
            };

            if (recipientByNameAndAddress.BirthDate != null)
            {
                nameAndAddressDto.birthdate = recipientByNameAndAddress.BirthDate.Value;
                nameAndAddressDto.birthdateSpecified = true;
            }

            return new messagerecipient
            {
                ItemElementName = ItemChoiceType1.nameandaddress,
                Item = nameAndAddressDto
            };
        }

        public static printdetails ToDataTransferObject(IPrintDetails printDetails)
        {
            if (printDetails == null)
                return null;

            var printDetailsDataTransferObject = new printdetails
            {
                recipient = ToDataTransferObject((IPrint)printDetails.PrintRecipient),
                returnaddress = ToDataTransferObject((IPrint)printDetails.PrintReturnRecipient),
                posttype = printDetails.PostType.ToPostType(),
                color = printDetails.PrintColors.ToPrintColors(),
                nondeliverablehandling = printDetails.NondeliverableHandling.ToNondeliverablehandling(),
            };

            return printDetailsDataTransferObject;
        }

        private static printrecipient ToDataTransferObject(IPrint recipient)
        {
            var printRecipientDto = new printrecipient()
            {
                name = recipient.Name
            };

            if (recipient.Address is INorwegianAddress)
            {
                printRecipientDto.Item = ToDataTransferObject((INorwegianAddress)recipient.Address);
            }
            else
            {
                printRecipientDto.Item = ToDataTransferObject((IForeignAddress)recipient.Address);
            }

            return printRecipientDto;
        }

        public static norwegianaddress ToDataTransferObject(INorwegianAddress norwegianAddress)
        {
            return new norwegianaddress
            {
                addressline1 = norwegianAddress.AddressLine1,
                addressline2 = norwegianAddress.AddressLine2,
                addressline3 = norwegianAddress.AddressLine3,
                zipcode = norwegianAddress.PostalCode,
                city = norwegianAddress.City
            };
        }

        public static foreignaddress ToDataTransferObject(IForeignAddress foreignAddress)
        {
            return new foreignaddress()
            {
                addressline1 = foreignAddress.AddressLine1,
                addressline2 = foreignAddress.AddressLine2,
                addressline3 = foreignAddress.AddressLine3,
                addressline4 = foreignAddress.Addressline4,
                ItemElementName = foreignAddress.CountryIdentifier.ToCountryIdentifier(),
                Item = foreignAddress.CountryIdentifierValue
            };
        }

        public static printrecipient ToDataTransferObject(Print.Print printOrPrintReturnRecipient)
        {
            printrecipient printRecipientDataTransferObject = new printrecipient
            {
                name = printOrPrintReturnRecipient.Name
            };

            var addressType = printOrPrintReturnRecipient.Address.GetType();

            if (typeof(INorwegianAddress).IsAssignableFrom(addressType))
            {
                var address = printOrPrintReturnRecipient.Address as NorwegianAddress;
                printRecipientDataTransferObject.Item = ToDataTransferObject(address);
            }
            else
            {
                var address = printOrPrintReturnRecipient.Address as ForeignAddress;
                printRecipientDataTransferObject.Item = ToDataTransferObject(address);
            };

            return printRecipientDataTransferObject;

        }

        public static smsnotification ToDataTransferObject(ISmsNotification smsNotification)
        {
            if (smsNotification == null)
                return null;

            var timesAsListedTimes = smsNotification.NotifyAtTimes.Select(dateTime => new listedtime { time = dateTime, timeSpecified = true });

            var smsNotificationDataTransferObject = new smsnotification
            {
                afterhours = smsNotification.NotifyAfterHours.ToArray(),
                at = timesAsListedTimes.ToArray()
            };

            return smsNotificationDataTransferObject;
        }

        public static IIdentificationResult FromDataTransferObject(identificationresult identificationResultDto)
        {
            IdentificationResult identificationResult;

            if (identificationResultDto.ItemsElementName.Length == 0) //IDENTIFICATION.NONE
            {
                identificationResult = SetResultTypeForIdentificationResultTypeNone(identificationResultDto);
            }
            else
            {
                ItemsChoiceType itemsChoiceType = identificationResultDto.ItemsElementName.ElementAt(0);


                identificationResult = new IdentificationResult(itemsChoiceType.ToIdentificationResultType() , identificationResultDto.Items.ElementAt(0).ToString());
            }

            return identificationResult;

        }

        private static IdentificationResult SetResultTypeForIdentificationResultTypeNone(identificationresult identificationResultDto)
        {
            IdentificationResult identificationResult = null;
            switch (identificationResultDto.result)
            {
                case identificationresultcode.DIGIPOST:
                    identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress, "");
                    break;
                case identificationresultcode.IDENTIFIED:
                    identificationResult = new IdentificationResult(IdentificationResultType.Personalias, "");
                    break;
            }
            return identificationResult;
        }

        public static IMessageDeliveryResult FromDataTransferObject(messagedelivery messageDeliveryDataTransferObject)
        {
            IMessageDeliveryResult messageDeliveryResult = new MessageDeliveryResult
            {
                PrimaryDocument = FromDataTransferObject(messageDeliveryDataTransferObject.primarydocument),
                Attachments = messageDeliveryDataTransferObject.attachment.Select(FromDataTransferObject).ToList(),
                DeliveryTime = messageDeliveryDataTransferObject.deliverytime,
                DeliveryMethod = messageDeliveryDataTransferObject.deliverymethod.ToDeliveryMethod(),
                Status = messageDeliveryDataTransferObject.status.ToMessageStatus()
            };

            return messageDeliveryResult;
        }

        public static IDocument FromDataTransferObject(document documentDto)
        {
            return new Document(
                documentDto.subject, 
                documentDto.filetype, 
                new byte[] {}, 
                documentDto.authenticationlevel.ToAuthenticationLevel(), 
                documentDto.sensitivitylevel.ToSensitivityLevel(), 
                FromDataTransferObject(documentDto.smsnotification))
                    {
                        Guid = documentDto.uuid
                    }; 
        }

        public static ISmsNotification FromDataTransferObject(smsnotification smsNotificationDataTransferObject)
        {
            if (smsNotificationDataTransferObject == null)
                return null;

            var dateTimes = smsNotificationDataTransferObject.at.Select(listedTime => listedTime.time).ToList();

            var smsNotification = new SmsNotification
            {
                NotifyAfterHours = smsNotificationDataTransferObject.afterhours.ToList(),
                NotifyAtTimes = dateTimes
            };

            return smsNotification;
        }
    }
}