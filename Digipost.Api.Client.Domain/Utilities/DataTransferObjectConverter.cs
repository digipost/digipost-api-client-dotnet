using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Extensions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.Utilities
{
    internal class DataTransferObjectConverter
    {
        public static identification ToDataTransferObject(IIdentification identification)
        {
            identification identificationDto = null;

            if (identification.DigipostRecipient is RecipientById)
            {
                identificationDto = IdentificationDataTransferObjectFromIdentificationById((RecipientById)identification.DigipostRecipient);
            }

            if (identification.DigipostRecipient is RecipientByNameAndAddress)
            {
                identificationDto = IdentificationDataTranferObjectFromIdentificationByNameAndAddress((RecipientByNameAndAddress)identification.DigipostRecipient);
            }

            return identificationDto;
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

        private static identification IdentificationDataTranferObjectFromIdentificationByNameAndAddress(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            var identification = new identification
            {
                ItemElementName = ItemChoiceType.nameandaddress,
                Item = new nameandaddress()
                {
                    fullname = recipientByNameAndAddress.FullName,
                    addressline1 = recipientByNameAndAddress.AddressLine1,
                    addressline2 = recipientByNameAndAddress.AddressLine2,
                    postalcode = recipientByNameAndAddress.PostalCode,
                    city = recipientByNameAndAddress.City,
                    emailaddress = recipientByNameAndAddress.Email,
                    phonenumber = recipientByNameAndAddress.PhoneNumber,
                },
                includepersonaliasfordigipostuser = true //Todo: Should be watwat?
            };

            if (recipientByNameAndAddress.BirthDate.HasValue)
            {
                var nameandaddress = ((nameandaddress)identification.Item);
                nameandaddress.birthdate = recipientByNameAndAddress.BirthDate.Value;
                nameandaddress.birthdateSpecified = true;
            }

            return identification;
        }

        public static message ToDataTransferObject(IMessage message)
        {
            var primaryDocumentDataTransferObject = ToDataTransferObject(message.PrimaryDocument);

            var messageDto = new message
            {
                Item = message.SenderId,
                primarydocument = primaryDocumentDataTransferObject,
                messageid = message.Id, 
                recipient = ToDataTransferObject(message.DigipostRecipient)
            };

            messageDto.recipient.printdetails = ToDataTransferObject(message.PrintDetails);
            
            if (message.DigipostRecipient is RecipientById)
            {
                var identificationType = ((RecipientById)message.DigipostRecipient).IdentificationType;
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

                var invoiceDto = new invoice();
                invoiceDto.amount = invoice.Amount;
                invoiceDto.duedate = invoice.Duedate;
                invoiceDto.kid = invoice.Kid;
                invoiceDto.account = invoice.Account;

                documentDto = invoiceDto;
            }
            else
            {
                documentDto = new document();
            }

            documentDto.subject = document.Subject;
            documentDto.filetype = document.FileType;
                // contenthash = new contenthash { hashalgorithm = "ALRGORITHM", Value = "VALUE" } //Todo: Hash the content bytes and add here
                //Item = document.ContentBytes, //Content bytes sendes gjennom en annen kanal
            documentDto.authenticationlevel = document.AuthenticationLevel.ToAuthenticationLevel();
            documentDto.authenticationlevelSpecified = true;
            documentDto.sensitivitylevel = document.SensitivityLevel.ToSensitivityLevel();
            documentDto.sensitivitylevelSpecified = true;
            documentDto.smsnotification = ToDataTransferObject(document.SmsNotification);
            documentDto.uuid = document.Guid;

            return documentDto;
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

            var smsNotificationDto = new smsnotification();

            if (smsNotification.NotifyAtTimes.Count > 0)
            {
                var timesAsListedTimes = smsNotification.NotifyAtTimes.Select(dateTime => new listedtime { time = dateTime, timeSpecified = true });
                smsNotificationDto.at = timesAsListedTimes.ToArray();
            }

            if(smsNotification.NotifyAfterHours.Count > 0)
            {
                smsNotificationDto.afterhours = smsNotification.NotifyAfterHours.ToArray();
            };

            return smsNotificationDto;
        }

        public static IIdentificationResult FromDataTransferObject(identificationresult identificationResultDto)
        {
            IdentificationResult identificationResult;

            var itemsChoiceType = identificationResultDto.ItemsElementName?.FirstOrDefault();
            var identifiedByDigipostOrPin = itemsChoiceType == null;

            if (identifiedByDigipostOrPin)
            {
                return IdentificationResultForDigipostOrPersonalIdentificationNumber(identificationResultDto);
            }
            else
            {
                identificationResult = new IdentificationResult(itemsChoiceType.Value.ToIdentificationResultType() , identificationResultDto.Items.ElementAt(0).ToString());
            }

            return identificationResult;

        }

        private static IdentificationResult IdentificationResultForDigipostOrPersonalIdentificationNumber(identificationresult identificationResultDto)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return identificationResult;
        }

        public static IMessageDeliveryResult FromDataTransferObject(messagedelivery messageDeliveryDto)
        {
            IMessageDeliveryResult messageDeliveryResult = new MessageDeliveryResult
            {
                PrimaryDocument = FromDataTransferObject(messageDeliveryDto.primarydocument),
                Attachments = messageDeliveryDto.attachment?.Select(FromDataTransferObject).ToList(),
                DeliveryTime = messageDeliveryDto.deliverytime,
                DeliveryMethod = messageDeliveryDto.deliverymethod.ToDeliveryMethod(),
                Status = messageDeliveryDto.status.ToMessageStatus()
            };

            return messageDeliveryResult;
        }

        public static IDocument FromDataTransferObject(document documentDto)
        {
            return new Document(documentDto.subject, documentDto.filetype, new byte[] {}, documentDto.authenticationlevel.ToAuthenticationLevel(), documentDto.sensitivitylevel.ToSensitivityLevel(), FromDataTransferObject(documentDto.smsnotification))
            {
                Guid = documentDto.uuid
            };
        }

        public static ISmsNotification FromDataTransferObject(smsnotification smsNotificationDto)
        {
            if (smsNotificationDto == null)
                return null;

            var smsNotification = new SmsNotification
            {
                NotifyAfterHours = smsNotificationDto.afterhours?.ToList() ?? new List<int>(), NotifyAtTimes = smsNotificationDto.at?.Select(listedTime => listedTime.time).ToList() ?? new List<DateTime>()
            };

            return smsNotification;
        }

        public static Error FromDataTransferObject(error error)
        {
            return new Error
            {
                Errorcode = error.errorcode,
                Errormessage = error.errormessage,
                Errortype = error.errortype
            };
        }

        public static SearchDetailsResult FromDataTransferObject(recipients recipients)
        {
            return new SearchDetailsResult
            {
                PersonDetails = recipients.recipient?.Select(
                    r => new SearchDetails
                    {
                        FirstName = r.firstname,
                        MiddleName = r.middlename,
                        LastName = r.lastname,
                        DigipostAddress = r.digipostaddress,
                        MobileNumber = r.mobilenumber,
                        OrganizationName = r.organisationname,
                        OrganizationNumber = r.organisationnumber,
                        SearchDetailsAddress = r.address?.Select(a => new SearchDetailsAddress
                        {
                            Street = a.street,
                            HouseNumber = a.housenumber,
                            HouseLetter = a.houseletter,
                            AdditionalAddressLine = a.additionaladdressline,
                            ZipCode = a.zipcode,
                            City = a.city
                        })
                    })
            };
        }
    }
}