using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Common.SenderInfo;
using Digipost.Api.Client.Common.Share;
using Digipost.Api.Client.Send;
using V8;
using Identification = V8.Identification;
using Link = Digipost.Api.Client.Common.Entrypoint.Link;

namespace Digipost.Api.Client.Common
{
    internal static class DataTransferObjectConverter
    {
        internal static Dictionary<string, Link> FromDataTransferObject(this IEnumerable<V8.Link> links)
        {
            return links.ToDictionary(
                l => l.Rel.Substring(l.Rel.LastIndexOf('/') + 1).ToUpper(),
                link => new Link(link.Uri) {Rel = link.Rel, MediaType = link.Media_Type}
            );
        }

        internal static DocumentEvents FromDataTransferObject(this Document_Events documentEvents)
        {
            return new DocumentEvents(documentEvents.Event.Select(FromDataTransferObject));
        }

        private static DocumentEvent FromDataTransferObject(this Event myEvent)
        {
            var documentEvent = new DocumentEvent(Guid.Parse(myEvent.Uuid), myEvent.Type.ToEventType(), myEvent.Created, myEvent.Document_Created);

            if (myEvent.Metadata is Request_For_Registration_Expired_Metadata forRegistrationExpiredMetadata)
            {
                documentEvent.EventMetadata = new RequestForRegistrationExpiredMetadata(forRegistrationExpiredMetadata.Fallback_Channel);
            }

            return documentEvent;
        }

        internal static Identification ToDataTransferObject(this IIdentification identification)
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

        private static Identification IdentificationDataTransferObjectFromIdentificationById(this RecipientById recipientById)
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

        private static Identification IdentificationDataTranferObjectFromIdentificationByNameAndAddress(this RecipientByNameAndAddress recipientByNameAndAddress)
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

        internal static Message_Recipient ToDataTransferObject(this IDigipostRecipient recipient)
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

        private static Message_Recipient RecipientDataTransferObjectFromRecipientById(this RecipientById recipient)
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

        private static Message_Recipient RecipientDataTransferObjectFromRecipientByNameAndAddress(this IRecipientByNameAndAddress recipientByNameAndAddress)
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

        private static List<Print_Instruction> ToDataTransferObject(this IPrintInstructions printInstructions)
        {
            if (printInstructions == null || printInstructions.PrintInstruction.Count == 0)
                return new List<Print_Instruction>();

            return printInstructions.PrintInstruction.Select(ToDataTransferObject).ToList();
        }

        private static Print_Instruction ToDataTransferObject(IPrintInstruction printInstruction)
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

        internal static Print_If_Unread ToDataTransferObject(this IPrintIfUnread printIfUnread)
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

        private static Print_Recipient ToDataTransferObject(this IPrint recipient)
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

        internal static Norwegian_Address ToDataTransferObject(this INorwegianAddress norwegianAddress)
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

        internal static Foreign_Address ToDataTransferObject(this IForeignAddress foreignAddress)
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
            }
            else if (foreignAddress.CountryIdentifier == CountryIdentifier.Countrycode)
            {
                result.Country_Code = foreignAddress.CountryIdentifierValue;
            }

            return result;
        }

        internal static Print_Recipient ToDataTransferObject(this Print.Print printOrPrintReturnRecipient)
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

        internal static Root FromDataTransferObject(this V8.Entrypoint entrypoint)
        {
            return new Root(entrypoint.Certificate)
            {
                Links = entrypoint.Link.FromDataTransferObject()
            };
        }

        internal static SenderInformation FromDataTransferObject(this Sender_Information dto)
        {
            var notValidSender = dto.Status == Sender_Status.NO_INFO_AVAILABLE;
            if (notValidSender)
            {
                return new SenderInformation(dto.Status.ToSenderStatus());
            }

            return new SenderInformation(
                new Sender(dto.Sender_Id),
                dto.Status.ToSenderStatus(),
                dto.Supported_Features.FromDataTransferObject()
            );
        }

        internal static SenderStatus ToSenderStatus(this Sender_Status dto)
        {
            switch (dto)
            {
                case Sender_Status.VALID_SENDER:
                    return SenderStatus.ValidSender;
                case Sender_Status.NO_INFO_AVAILABLE:
                    return SenderStatus.NoInfoAvailable;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static IEnumerable<SenderFeature> FromDataTransferObject(this Collection<Feature> dto)
        {
            return dto.Select(d => new SenderFeature(d.Value, d.Param));
        }

        internal static IIdentificationResult FromDataTransferObject(this Identification_Result identificationResultDto)
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
                return new IdentificationResult(IdentificationResultType.InvalidReason, identificationResultDto.Invalid_Reason.ToIdentificationError());
            }

            if (identificationResultDto.Unidentified_ReasonSpecified)
            {
                return new IdentificationResult(IdentificationResultType.UnidentifiedReason, identificationResultDto.Unidentified_Reason.ToIdentificationError());
            }

            throw new ArgumentOutOfRangeException(nameof(identificationResultDto.Result), identificationResultDto.Result, null);
        }

        private static IdentificationResult IdentificationResultForDigipostOrPersonalIdentificationNumber(this Identification_Result identificationResultDto)
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

        internal static Error FromDataTransferObject(this V8.Error error)
        {
            return new Error
            {
                Errorcode = error.Error_Code,
                Errormessage = error.Error_Message,
                Errortype = error.Error_Type
            };
        }

        internal static SearchDetailsResult FromDataTransferObject(this Recipients recipients)
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


        internal static ShareDocumentsRequestState FromDataTransferObject(this Share_Documents_Request_State dto)
        {
            return new ShareDocumentsRequestState(
                dto.Shared_At_TimeSpecified ? dto.Shared_At_Time : (DateTime?) null,
                dto.Expiry_TimeSpecified ? dto.Expiry_Time : (DateTime?) null,
                dto.Withdrawn_TimeSpecified ? dto.Withdrawn_Time : (DateTime?) null,
                dto.Shared_Documents.FromDataTransferObject(),
                dto.Link.FromDataTransferObject()
            );
        }

        private static IEnumerable<SharedDocument> FromDataTransferObject(this Collection<Shared_Document> dto)
        {
            return dto.Select(document => new SharedDocument(
                document.Delivery_Time,
                document.Subject,
                document.File_Type,
                Convert.ToInt32(document.File_Size_Bytes),
                document.Origin.FromDataTransferObject(),
                document.Link.FromDataTransferObject()
            )).ToList();
        }

        private static IOrigin FromDataTransferObject(this Shared_Document_Origin dto)
        {
            if (dto.Organisation != null)
            {
                return new OrganisationOrigin(dto.Organisation.Name, dto.Organisation.Organisation_Number);
            }

            return new PrivatePersonOrigin(dto.Private_Person.Name);
        }

        internal static SharedDocumentContent FromDataTransferObject(this Shared_Document_Content dto)
        {
            return new SharedDocumentContent(dto.Content_Type, new Uri(dto.Uri, UriKind.Absolute));
        }
    }
}
