using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Send;
using V8;
using Identification = V8.Identification;
using Link = Digipost.Api.Client.Common.Entrypoint.Link;
using Root = Digipost.Api.Client.Common.Entrypoint.Root;

namespace Digipost.Api.Client.Common
{
    internal static class DataTransferObjectConverter
    {
        public static Dictionary<string, Link> FromDataTransferObject(IEnumerable<V8.Link> links)
        {
            return links.ToDictionary(
                l => l.Rel.Substring(l.Rel.LastIndexOf('/') + 1).ToUpper(),
                link => new Link(link.Uri) {Rel = link.Rel, MediaType = link.Media_Type}
            );
        }

        public static Identification ToDataTransferObject(IIdentification identification)
        {
            Identification identificationDto = null;

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

        private static Identification IdentificationDataTransferObjectFromIdentificationById(RecipientById recipientById)
        {
            switch (recipientById.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new Identification {Digipost_Address = recipientById.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new Identification {Personal_Identification_Number = recipientById.Id};
                case IdentificationType.OrganizationNumber:
                    return new Identification {Organisation_Number = recipientById.Id};
                case IdentificationType.BankAccountNumber:
                    return new Identification {Bank_Account_Number = recipientById.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipientById.IdentificationType), recipientById.IdentificationType, null);
            }
        }

        private static Identification IdentificationDataTranferObjectFromIdentificationByNameAndAddress(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            var identification = new Identification
            {
                Name_And_Address = new Name_And_Address()
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

        public static Message_Recipient Message_RecipientDataTranferObjectFromIdentificationByNameAndAddress(RecipientByNameAndAddress recipientByNameAndAddress)
        {
            var identification = new Message_Recipient
            {
                Name_And_Address = new Name_And_Address()
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

        public static Message_Recipient Message_RecipientDataTransferObjectFromIdentificationById(RecipientById recipientById)
        {
            switch (recipientById.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new Message_Recipient {Digipost_Address = recipientById.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new Message_Recipient {Personal_Identification_Number = recipientById.Id};
                case IdentificationType.OrganizationNumber:
                    return new Message_Recipient {Organisation_Number = recipientById.Id};
                case IdentificationType.BankAccountNumber:
                    return new Message_Recipient {Bank_Account_Number = recipientById.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipientById.IdentificationType), recipientById.IdentificationType, null);
            }
        }


        public static Message_Recipient ToDataTransferObject(IDigipostRecipient recipient)
        {
            Message_Recipient messageRecipientDto = null;

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

        private static Message_Recipient RecipientDataTransferObjectFromRecipientById(RecipientById recipient)
        {
            switch (recipient.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new Message_Recipient {Digipost_Address = recipient.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new Message_Recipient {Personal_Identification_Number = recipient.Id};
                case IdentificationType.OrganizationNumber:
                    return new Message_Recipient {Organisation_Number = recipient.Id};
                case IdentificationType.BankAccountNumber:
                    return new Message_Recipient {Bank_Account_Number = recipient.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipient.IdentificationType), recipient.IdentificationType, null);
            }
        }

        private static Message_Recipient RecipientDataTransferObjectFromRecipientByNameAndAddress(IRecipientByNameAndAddress recipientByNameAndAddress)
        {
            var nameAndAddressDto = new Name_And_Address
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

            return new Message_Recipient
            {
                Name_And_Address = nameAndAddressDto
            };
        }

        internal static Print_Details ToDataTransferObject(this IPrintDetails printDetails)
        {
            if (printDetails == null)
                return null;

            var printDetailsDataTransferObject = new Print_Details
            {
                Recipient = ToDataTransferObject((IPrint) printDetails.PrintRecipient),
                Return_Address = ToDataTransferObject((IPrint) printDetails.PrintReturnRecipient),
                Color = printDetails.PrintColors.ToPrintColors(),
                Nondeliverable_Handling = printDetails.NondeliverableHandling.ToNondeliverablehandling(),
            };

            ToDataTransferObject(printDetails.PrintInstructions).ForEach(printDetailsDataTransferObject.Print_Instructions.Add);

            return printDetailsDataTransferObject;
        }

        public static List<Print_Instruction> ToDataTransferObject(IPrintInstructions printInstructions)
        {
            if (printInstructions == null || printInstructions.PrintInstruction.Count == 0)
                return new List<Print_Instruction>();

            return printInstructions.PrintInstruction.Select(ToDataTransferObject).ToList();
        }

        public static Print_Instruction ToDataTransferObject(IPrintInstruction printInstruction)
        {
            if (printInstruction == null)
                return null;

            var printInstructionTransferObject = new Print_Instruction
            {
                Key = printInstruction.key,
                Value = printInstruction.value
            };

            return printInstructionTransferObject;
        }

        public static Print_If_Unread ToDataTransferObject(IPrintIfUnread printIfUnread)
        {
            if (printIfUnread == null)
                return null;

            var printIfUnreadDataTransferObject = new Print_If_Unread
            {
                Print_If_Unread_After = printIfUnread.PrintIfUnreadAfter,
                Print_Details = ToDataTransferObject(printIfUnread.PrintDetails)
            };

            return printIfUnreadDataTransferObject;
        }

        internal static Request_For_Registration ToDataTransferObject(this RequestForRegistration requestForRegistration)
        {
            return new Request_For_Registration
            {
                Registration_Deadline = requestForRegistration.RegistrationDeadline,
                Phone_Number = requestForRegistration.PhoneNumber,
                Email_Address = requestForRegistration.EmailAddress,
                Print_Details = requestForRegistration.PrintDetails.ToDataTransferObject()
            };
        }

        private static Print_Recipient ToDataTransferObject(IPrint recipient)
        {
            var printRecipientDto = new Print_Recipient
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

        public static Norwegian_Address ToDataTransferObject(INorwegianAddress norwegianAddress)
        {
            return new Norwegian_Address
            {
                Addressline1 = norwegianAddress.AddressLine1,
                Addressline2 = norwegianAddress.AddressLine2,
                Addressline3 = norwegianAddress.AddressLine3,
                Zip_Code = norwegianAddress.PostalCode,
                City = norwegianAddress.City
            };
        }

        public static Foreign_Address ToDataTransferObject(IForeignAddress foreignAddress)
        {
            var result = new Foreign_Address
            {
                Addressline1 = foreignAddress.AddressLine1,
                Addressline2 = foreignAddress.AddressLine2,
                Addressline3 = foreignAddress.AddressLine3,
                Addressline4 = foreignAddress.AddressLine4,
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

        public static Print_Recipient ToDataTransferObject(Print.Print printOrPrintReturnRecipient)
        {
            var printRecipientDataTransferObject = new Print_Recipient
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

        public static Root FromDataTransferObject(V8.Entrypoint entrypoint)
        {
            return new Root(entrypoint.Certificate)
            {
                Links = FromDataTransferObject(entrypoint.Link)
            };
        }

        public static IIdentificationResult FromDataTransferObject(Identification_Result identificationResultDto)
        {
            var digipostAddress = identificationResultDto.Digipost_Address;
            var personAlias = identificationResultDto.Person_Alias;

            var identifiedByDigipostOrPin = digipostAddress != null || personAlias != null || identificationResultDto.Result == Identification_Result_Code.DIGIPOST || identificationResultDto.Result == Identification_Result_Code.IDENTIFIED;

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

        private static IdentificationResult IdentificationResultForDigipostOrPersonalIdentificationNumber(Identification_Result identificationResultDto)
        {
            IdentificationResult identificationResult;

            switch (identificationResultDto.Result)
            {
                case Identification_Result_Code.DIGIPOST:
                    identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress, identificationResultDto.Digipost_Address + "");
                    break;
                case Identification_Result_Code.IDENTIFIED:
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

        public static SearchDetailsResult FromDataTransferObject(Recipients recipients)
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

        public static HashAlgoritm ToHashAlgoritm(this V8.Hash_Algorithm hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case Hash_Algorithm.NONE:
                    return HashAlgoritm.NONE;
                case Hash_Algorithm.MD5:
                    return HashAlgoritm.MD5;
                case Hash_Algorithm.SHA256:
                    return HashAlgoritm.SHA256;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
