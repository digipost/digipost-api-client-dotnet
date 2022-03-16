using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Search;
using V8;

namespace Digipost.Api.Client.Common
{
    internal class DataTransferObjectConverter
    {
        public static V8.Identification ToDataTransferObject(IIdentification identification)
        {
            V8.Identification identificationDto = null;

            if (identification.DigipostRecipient is RecipientById)
            {
                identificationDto = IdentificationDataTransferObjectFromIdentificationById((RecipientById) identification.DigipostRecipient);
            }

            if (identification.DigipostRecipient is RecipientByNameAndAddress)
            {
                identificationDto = IdentificationDataTranferObjectFromIdentificationByNameAndAddress((RecipientByNameAndAddress) identification.DigipostRecipient);
            }

            return identificationDto;
        }

        private static V8.Identification IdentificationDataTransferObjectFromIdentificationById(RecipientById recipientById)
        {
            switch (recipientById.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new V8.Identification {Digipost_Address = recipientById.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new V8.Identification {Personal_Identification_Number = recipientById.Id};
                case IdentificationType.OrganizationNumber:
                    return new V8.Identification {Organisation_Number = recipientById.Id};
                case IdentificationType.BankAccountNumber:
                    return new V8.Identification {Bank_Account_Number = recipientById.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipientById.IdentificationType), recipientById.IdentificationType, null);
            }
        }

        private static V8.Identification IdentificationDataTranferObjectFromIdentificationByNameAndAddress(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            var identification = new V8.Identification
            {
                Name_And_Address = new V8.Name_And_Address()
                {
                    Fullname = recipientByNameAndAddress.FullName,
                    Addressline1 = recipientByNameAndAddress.AddressLine1,
                    Addressline2 = recipientByNameAndAddress.AddressLine2,
                    Postalcode = recipientByNameAndAddress.PostalCode,
                    City = recipientByNameAndAddress.City,
                    Email_Address = recipientByNameAndAddress.Email,
                    Phone_Number = recipientByNameAndAddress.PhoneNumber
                }
            };

            if (recipientByNameAndAddress.BirthDate.HasValue)
            {
                var nameandaddress = identification.Name_And_Address;
                nameandaddress.Birth_Date = recipientByNameAndAddress.BirthDate.Value;
                nameandaddress.Birth_DateSpecified = true;
            }

            return identification;
        }

        public static V8.Message_Recipient Message_RecipientDataTranferObjectFromIdentificationByNameAndAddress(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            var identification = new V8.Message_Recipient
            {
                Name_And_Address = new V8.Name_And_Address()
                {
                    Fullname = recipientByNameAndAddress.FullName,
                    Addressline1 = recipientByNameAndAddress.AddressLine1,
                    Addressline2 = recipientByNameAndAddress.AddressLine2,
                    Postalcode = recipientByNameAndAddress.PostalCode,
                    City = recipientByNameAndAddress.City,
                    Email_Address = recipientByNameAndAddress.Email,
                    Phone_Number = recipientByNameAndAddress.PhoneNumber
                }
            };

            if (recipientByNameAndAddress.BirthDate.HasValue)
            {
                var nameandaddress = identification.Name_And_Address;
                nameandaddress.Birth_Date = recipientByNameAndAddress.BirthDate.Value;
                nameandaddress.Birth_DateSpecified = true;
            }

            return identification;
        }

        public static V8.Message_Recipient Message_RecipientDataTransferObjectFromIdentificationById(RecipientById recipientById)
        {
            switch (recipientById.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new V8.Message_Recipient {Digipost_Address = recipientById.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new V8.Message_Recipient {Personal_Identification_Number = recipientById.Id};
                case IdentificationType.OrganizationNumber:
                    return new V8.Message_Recipient {Organisation_Number = recipientById.Id};
                case IdentificationType.BankAccountNumber:
                    return new V8.Message_Recipient {Bank_Account_Number = recipientById.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipientById.IdentificationType), recipientById.IdentificationType, null);
            }
        }


        public static V8.Message_Recipient ToDataTransferObject(IDigipostRecipient recipient)
        {
            V8.Message_Recipient messageRecipientDto = null;

            if (recipient is RecipientById)
            {
                messageRecipientDto = RecipientDataTransferObjectFromRecipientById((RecipientById) recipient);
            }

            if (recipient is RecipientByNameAndAddress)
            {
                messageRecipientDto = RecipientDataTransferObjectFromRecipientByNameAndAddress((RecipientByNameAndAddress) recipient);
            }

            return messageRecipientDto;
        }

        private static V8.Message_Recipient RecipientDataTransferObjectFromRecipientById(RecipientById recipient)
        {
            switch (recipient.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new V8.Message_Recipient {Digipost_Address = recipient.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new V8.Message_Recipient {Personal_Identification_Number = recipient.Id};
                case IdentificationType.OrganizationNumber:
                    return new V8.Message_Recipient {Organisation_Number = recipient.Id};
                case IdentificationType.BankAccountNumber:
                    return new V8.Message_Recipient {Bank_Account_Number = recipient.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipient.IdentificationType), recipient.IdentificationType, null);
            }
        }

        private static V8.Message_Recipient RecipientDataTransferObjectFromRecipientByNameAndAddress(IRecipientByNameAndAddress recipientByNameAndAddress)
        {
            var nameAndAddressDto = new V8.Name_And_Address()
            {
                Fullname = recipientByNameAndAddress.FullName,
                Addressline1 = recipientByNameAndAddress.AddressLine1,
                Addressline2 = recipientByNameAndAddress.AddressLine2,
                Postalcode = recipientByNameAndAddress.PostalCode,
                City = recipientByNameAndAddress.City,
                Email_Address = recipientByNameAndAddress.Email,
                Phone_Number = recipientByNameAndAddress.PhoneNumber
            };

            if (recipientByNameAndAddress.BirthDate != null)
            {
                nameAndAddressDto.Birth_Date = recipientByNameAndAddress.BirthDate.Value;
                nameAndAddressDto.Birth_DateSpecified = true;
            }

            return new V8.Message_Recipient
            {
                Name_And_Address = nameAndAddressDto
            };
        }

        public static V8.Print_Details ToDataTransferObject(IPrintDetails printDetails)
        {
            if (printDetails == null)
                return null;

            var printDetailsDataTransferObject = new V8.Print_Details
            {
                Recipient = ToDataTransferObject((IPrint) printDetails.PrintRecipient),
                Return_Address = ToDataTransferObject((IPrint) printDetails.PrintReturnRecipient),
                Color = printDetails.PrintColors.ToPrintColors(),
                Nondeliverable_Handling = printDetails.NondeliverableHandling.ToNondeliverablehandling(),
            };

            ToDataTransferObject(printDetails.PrintInstructions).ForEach(printDetailsDataTransferObject.Print_Instructions.Add);

            return printDetailsDataTransferObject;
        }

        public static List<V8.Print_Instruction> ToDataTransferObject(IPrintInstructions printInstructions)
        {
            if (printInstructions == null || printInstructions.PrintInstruction.Count == 0)
                return new List<Print_Instruction>();

            return printInstructions.PrintInstruction.Select(ToDataTransferObject).ToList();
        }

        public static V8.Print_Instruction ToDataTransferObject(IPrintInstruction printInstruction)
        {
            if (printInstruction == null)
                return null;

            var printInstructionTransferObject = new V8.Print_Instruction
            {
                Key = printInstruction.key,
                Value = printInstruction.value
            };

            return printInstructionTransferObject;
        }

        public static V8.Print_If_Unread ToDataTransferObject(IPrintIfUnread printIfUnread)
        {
            if (printIfUnread == null)
                return null;

            var printIfUnreadDataTransferObject = new V8.Print_If_Unread
            {
                Print_If_Unread_After = printIfUnread.PrintIfUnreadAfter,
                Print_Details = ToDataTransferObject(printIfUnread.PrintDetails)
            };

            return printIfUnreadDataTransferObject;
        }

        private static V8.Print_Recipient ToDataTransferObject(IPrint recipient)
        {
            var printRecipientDto = new V8.Print_Recipient
            {
                Name = recipient.Name
            };

            if (recipient.Address is INorwegianAddress)
            {
                printRecipientDto.Norwegian_Address = ToDataTransferObject((INorwegianAddress) recipient.Address);
            }
            else
            {
                printRecipientDto.Foreign_Address = ToDataTransferObject((IForeignAddress) recipient.Address);
            }

            return printRecipientDto;
        }

        public static V8.Norwegian_Address ToDataTransferObject(INorwegianAddress norwegianAddress)
        {
            return new V8.Norwegian_Address
            {
                Addressline1 = norwegianAddress.AddressLine1,
                Addressline2 = norwegianAddress.AddressLine2,
                Addressline3 = norwegianAddress.AddressLine3,
                Zip_Code = norwegianAddress.PostalCode,
                City = norwegianAddress.City
            };
        }

        public static V8.Foreign_Address ToDataTransferObject(IForeignAddress foreignAddress)
        {
            var result = new V8.Foreign_Address
            {
                Addressline1 = foreignAddress.AddressLine1,
                Addressline2 = foreignAddress.AddressLine2,
                Addressline3 = foreignAddress.AddressLine3,
                Addressline4 = foreignAddress.Addressline4,
            };

            if (foreignAddress.CountryIdentifier == CountryIdentifier.Country)
            {
                result.Country = foreignAddress.CountryIdentifierValue;
            } else if (foreignAddress.CountryIdentifier == CountryIdentifier.Countrycode)
            {
                result.Country_Code = foreignAddress.CountryIdentifierValue;
            }

            return result;
        }

        public static V8.Print_Recipient ToDataTransferObject(Print.Print printOrPrintReturnRecipient)
        {
            var printRecipientDataTransferObject = new V8.Print_Recipient
            {
                Name = printOrPrintReturnRecipient.Name
            };

            var addressType = printOrPrintReturnRecipient.Address.GetType();

            if (typeof(INorwegianAddress).IsAssignableFrom(addressType))
            {
                var address = printOrPrintReturnRecipient.Address as NorwegianAddress;
                printRecipientDataTransferObject.Norwegian_Address = ToDataTransferObject(address);
            }
            else
            {
                var address = printOrPrintReturnRecipient.Address as ForeignAddress;
                printRecipientDataTransferObject.Foreign_Address = ToDataTransferObject(address);
            }

            return printRecipientDataTransferObject;
        }

        public static V8.Sms_Notification ToDataTransferObject(ISmsNotification smsNotification)
        {
            if (smsNotification == null)
                return null;

            var smsNotificationDto = new V8.Sms_Notification();

            if (smsNotification.NotifyAtTimes.Count > 0)
            {
                var timesAsListedTimes = smsNotification.NotifyAtTimes.Select(dateTime => new V8.Listed_Time {Time = dateTime, TimeSpecified = true});
                foreach (var timesAsListedTime in timesAsListedTimes)
                {
                    smsNotificationDto.At.Add(timesAsListedTime);
                }
            }

            if (smsNotification.NotifyAfterHours.Count > 0)
            {
                foreach (var i in smsNotification.NotifyAfterHours.ToArray())
                {
                    smsNotificationDto.After_Hours.Add(i);
                }
            }

            return smsNotificationDto;
        }

        public static IIdentificationResult FromDataTransferObject(V8.Identification_Result identificationResultDto)
        {
            var digipostAddress = identificationResultDto.Digipost_Address;
            var personAlias = identificationResultDto.Person_Alias;

            var identifiedByDigipostOrPin = digipostAddress != null || personAlias != null || identificationResultDto.Result == V8.Identification_Result_Code.DIGIPOST || identificationResultDto.Result == V8.Identification_Result_Code.IDENTIFIED;

            if (identifiedByDigipostOrPin)
            {
                return IdentificationResultForDigipostOrPersonalIdentificationNumber(identificationResultDto);
            }

            if (identificationResultDto.Invalid_ReasonSpecified)
            {
                return new IdentificationResult(IdentificationResultType.InvalidReason, identificationResultDto.Invalid_Reason.ToString());
            }
            if (identificationResultDto.Unidentified_ReasonSpecified)
            {
                return new IdentificationResult(IdentificationResultType.UnidentifiedReason, identificationResultDto.Unidentified_Reason.ToString());
            }

            throw new ArgumentOutOfRangeException(nameof(identificationResultDto.Result), identificationResultDto.Result, null);
        }

        private static IdentificationResult IdentificationResultForDigipostOrPersonalIdentificationNumber(V8.Identification_Result identificationResultDto)
        {
            IdentificationResult identificationResult;

            switch (identificationResultDto.Result)
            {
                case V8.Identification_Result_Code.DIGIPOST:
                    identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress, identificationResultDto.Digipost_Address + "");
                    break;
                case V8.Identification_Result_Code.IDENTIFIED:
                    identificationResult = new IdentificationResult(IdentificationResultType.Personalias, identificationResultDto.Person_Alias + "");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return identificationResult;
        }

        public static Error FromDataTransferObject(V8.Error error)
        {
            return new Error
            {
                Errorcode = error.Error_Code,
                Errormessage = error.Error_Message,
                Errortype = error.Error_Type
            };
        }

        public static SearchDetailsResult FromDataTransferObject(V8.Recipients recipients)
        {
            return new SearchDetailsResult
            {
                PersonDetails = recipients.Recipient?.Select(
                    r => new SearchDetails
                    {
                        FirstName = r.Firstname,
                        MiddleName = r.Middlename,
                        LastName = r.Lastname,
                        DigipostAddress = r.Digipost_Address,
                        MobileNumber = r.Mobile_Number,
                        OrganizationName = r.Organisation_Name,
                        OrganizationNumber = r.Organisation_Number,
                        SearchDetailsAddress = r.Address?.Select(a => new SearchDetailsAddress
                        {
                            Street = a.Street,
                            HouseNumber = a.House_Number,
                            HouseLetter = a.House_Letter,
                            AdditionalAddressLine = a.Additional_Addressline,
                            PostalCode = a.Zip_Code,
                            City = a.City
                        })
                    })
            };
        }
    }
}
