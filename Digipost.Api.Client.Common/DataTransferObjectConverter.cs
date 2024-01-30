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
using V8 = Digipost.Api.Client.Common.Generated.V8;
using Link = Digipost.Api.Client.Common.Entrypoint.Link;

namespace Digipost.Api.Client.Common
{
    internal static class DataTransferObjectConverter
    {
        internal static Dictionary<string, Link> FromDataTransferObject(this IEnumerable<V8.Link> links)
        {
            return links.ToDictionary(
                l => l.Rel.Substring(l.Rel.LastIndexOf('/') + 1).ToUpper(),
                link => new Link(link.Uri) {Rel = link.Rel, MediaType = link.MediaType}
            );
        }

        internal static DocumentEvents FromDataTransferObject(this V8.DocumentEvents documentEvents)
        {
            return new DocumentEvents(documentEvents.Event.Select(FromDataTransferObject));
        }

        private static DocumentEvent FromDataTransferObject(this V8.Event myEvent)
        {
            var documentEvent = new DocumentEvent(Guid.Parse(myEvent.Uuid), myEvent.Type.ToEventType(), myEvent.Created, myEvent.DocumentCreated);

            if (myEvent.Metadata is V8.RequestForRegistrationExpiredMetadata forRegistrationExpiredMetadata)
            {
                documentEvent.EventMetadata = new RequestForRegistrationExpiredMetadata(forRegistrationExpiredMetadata.FallbackChannel);
            }

            return documentEvent;
        }

        internal static V8.Identification ToDataTransferObject(this IIdentification identification)
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

        private static V8.Identification IdentificationDataTransferObjectFromIdentificationById(this RecipientById recipientById)
        {
            switch (recipientById.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new V8.Identification {DigipostAddress = recipientById.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new V8.Identification {PersonalIdentificationNumber = recipientById.Id};
                case IdentificationType.OrganizationNumber:
                    return new V8.Identification {OrganisationNumber = recipientById.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipientById.IdentificationType), recipientById.IdentificationType, null);
            }
        }

        private static V8.Identification IdentificationDataTranferObjectFromIdentificationByNameAndAddress(this RecipientByNameAndAddress recipientByNameAndAddress)
        {
            var identification = new V8.Identification
            {
                NameAndAddress = new V8.NameAndAddress()
                {
                    Fullname = recipientByNameAndAddress.FullName,
                    Addressline1 = recipientByNameAndAddress.AddressLine1,
                    Addressline2 = recipientByNameAndAddress.AddressLine2,
                    Postalcode = recipientByNameAndAddress.PostalCode,
                    City = recipientByNameAndAddress.City,
                    EmailAddress = recipientByNameAndAddress.Email,
                    PhoneNumber = recipientByNameAndAddress.PhoneNumber
                }
            };

            if (recipientByNameAndAddress.BirthDate.HasValue)
            {
                var nameandaddress = identification.NameAndAddress;
                nameandaddress.BirthDate = recipientByNameAndAddress.BirthDate.Value;
                nameandaddress.BirthDateSpecified = true;
            }

            return identification;
        }

        internal static V8.MessageRecipient ToDataTransferObject(this IDigipostRecipient recipient)
        {
            V8.MessageRecipient messageRecipientDto = null;

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

        private static V8.MessageRecipient RecipientDataTransferObjectFromRecipientById(this RecipientById recipient)
        {
            switch (recipient.IdentificationType)
            {
                case IdentificationType.DigipostAddress:
                    return new V8.MessageRecipient {DigipostAddress = recipient.Id};
                case IdentificationType.PersonalIdentificationNumber:
                    return new V8.MessageRecipient {PersonalIdentificationNumber = recipient.Id};
                case IdentificationType.OrganizationNumber:
                    return new V8.MessageRecipient {OrganisationNumber = recipient.Id};
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipient.IdentificationType), recipient.IdentificationType, null);
            }
        }

        private static V8.MessageRecipient RecipientDataTransferObjectFromRecipientByNameAndAddress(this IRecipientByNameAndAddress recipientByNameAndAddress)
        {
            var nameAndAddressDto = new V8.NameAndAddress()
            {
                Fullname = recipientByNameAndAddress.FullName,
                Addressline1 = recipientByNameAndAddress.AddressLine1,
                Addressline2 = recipientByNameAndAddress.AddressLine2,
                Postalcode = recipientByNameAndAddress.PostalCode,
                City = recipientByNameAndAddress.City,
                EmailAddress = recipientByNameAndAddress.Email,
                PhoneNumber = recipientByNameAndAddress.PhoneNumber
            };

            if (recipientByNameAndAddress.BirthDate != null)
            {
                nameAndAddressDto.BirthDate = recipientByNameAndAddress.BirthDate.Value;
                nameAndAddressDto.BirthDateSpecified = true;
            }

            return new V8.MessageRecipient
            {
                NameAndAddress = nameAndAddressDto
            };
        }

        internal static V8.PrintDetails ToDataTransferObject(this IPrintDetails printDetails)
        {
            if (printDetails == null)
                return null;

            var printDetailsDataTransferObject = new V8.PrintDetails
            {
                Recipient = ToDataTransferObject((IPrint) printDetails.PrintRecipient),
                ReturnAddress = ToDataTransferObject((IPrint) printDetails.PrintReturnRecipient),
                Color = printDetails.PrintColors.ToPrintColors(),
                NondeliverableHandling = printDetails.NondeliverableHandling.ToNondeliverablehandling(),
            };

            ToDataTransferObject(printDetails.PrintInstructions).ForEach(printDetailsDataTransferObject.PrintInstructions.Add);

            return printDetailsDataTransferObject;
        }

        private static List<V8.PrintInstruction> ToDataTransferObject(this IPrintInstructions printInstructions)
        {
            if (printInstructions == null || printInstructions.PrintInstruction.Count == 0)
                return new List<V8.PrintInstruction>();

            return printInstructions.PrintInstruction.Select(ToDataTransferObject).ToList();
        }

        private static V8.PrintInstruction ToDataTransferObject(IPrintInstruction printInstruction)
        {
            if (printInstruction == null)
                return null;

            var printInstructionTransferObject = new V8.PrintInstruction
            {
                Key = printInstruction.key,
                Value = printInstruction.value
            };

            return printInstructionTransferObject;
        }

        internal static V8.PrintIfUnread ToDataTransferObject(this IPrintIfUnread printIfUnread)
        {
            if (printIfUnread == null)
                return null;

            var printIfUnreadDataTransferObject = new V8.PrintIfUnread
            {
                PrintIfUnreadAfter = printIfUnread.PrintIfUnreadAfter,
                PrintDetails = ToDataTransferObject(printIfUnread.PrintDetails)
            };

            return printIfUnreadDataTransferObject;
        }

        internal static V8.RequestForRegistration ToDataTransferObject(this RequestForRegistration requestForRegistration)
        {
            return new V8.RequestForRegistration
            {
                RegistrationDeadline = requestForRegistration.RegistrationDeadline,
                PhoneNumber = requestForRegistration.PhoneNumber,
                EmailAddress = requestForRegistration.EmailAddress,
                PrintDetails = requestForRegistration.PrintDetails.ToDataTransferObject()
            };
        }

        private static V8.PrintRecipient ToDataTransferObject(this IPrint recipient)
        {
            var printRecipientDto = new V8.PrintRecipient
            {
                Name = recipient.Name
            };

            if (recipient.Address is INorwegianAddress)
            {
                printRecipientDto.NorwegianAddress = ToDataTransferObject((INorwegianAddress) recipient.Address);
            }
            else
            {
                printRecipientDto.ForeignAddress = ToDataTransferObject((IForeignAddress) recipient.Address);
            }

            return printRecipientDto;
        }

        internal static V8.NorwegianAddress ToDataTransferObject(this INorwegianAddress norwegianAddress)
        {
            return new V8.NorwegianAddress
            {
                Addressline1 = norwegianAddress.AddressLine1,
                Addressline2 = norwegianAddress.AddressLine2,
                Addressline3 = norwegianAddress.AddressLine3,
                ZipCode = norwegianAddress.PostalCode,
                City = norwegianAddress.City
            };
        }

        internal static V8.ForeignAddress ToDataTransferObject(this IForeignAddress foreignAddress)
        {
            var result = new V8.ForeignAddress
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
                result.CountryCode = foreignAddress.CountryIdentifierValue;
            }

            return result;
        }

        internal static V8.PrintRecipient ToDataTransferObject(this Print.Print printOrPrintReturnRecipient)
        {
            var printRecipientDataTransferObject = new V8.PrintRecipient
            {
                Name = printOrPrintReturnRecipient.Name
            };

            var addressType = printOrPrintReturnRecipient.Address.GetType();

            if (typeof(INorwegianAddress).IsAssignableFrom(addressType))
            {
                var address = printOrPrintReturnRecipient.Address as NorwegianAddress;
                printRecipientDataTransferObject.NorwegianAddress = ToDataTransferObject(address);
            }
            else
            {
                var address = printOrPrintReturnRecipient.Address as ForeignAddress;
                printRecipientDataTransferObject.ForeignAddress = ToDataTransferObject(address);
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

        internal static SenderInformation FromDataTransferObject(this V8.SenderInformation dto)
        {
            var notValidSender = dto.Status == V8.SenderStatus.NoInfoAvailable;
            if (notValidSender)
            {
                return new SenderInformation(dto.Status.ToSenderStatus());
            }

            return new SenderInformation(
                new Sender(dto.SenderId),
                dto.Status.ToSenderStatus(),
                dto.SupportedFeatures.FromDataTransferObject()
            );
        }

        internal static SenderStatus ToSenderStatus(this V8.SenderStatus dto)
        {
            switch (dto)
            {
                case V8.SenderStatus.ValidSender:
                    return SenderStatus.ValidSender;
                case V8.SenderStatus.NoInfoAvailable:
                    return SenderStatus.NoInfoAvailable;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static IEnumerable<SenderFeature> FromDataTransferObject(this Collection<V8.Feature> dto)
        {
            return dto.Select(d => new SenderFeature(d.Value, d.Param));
        }

        internal static IIdentificationResult FromDataTransferObject(this V8.IdentificationResult identificationResultDto)
        {
            var digipostAddress = identificationResultDto.DigipostAddress;
            var personAlias = !identificationResultDto.PersonAlias.Any() ? null : identificationResultDto.PersonAlias.First();

            var identifiedByDigipostOrPin = digipostAddress != null || personAlias != null || identificationResultDto.Result == V8.IdentificationResultCode.Digipost || identificationResultDto.Result == V8.IdentificationResultCode.Identified;

            if (identifiedByDigipostOrPin)
            {
                return IdentificationResultForDigipostOrPersonalIdentificationNumber(identificationResultDto);
            }

            if (identificationResultDto.InvalidReasonSpecified)
            {
                return new IdentificationResult(IdentificationResultType.InvalidReason, identificationResultDto.InvalidReason.ToIdentificationError());
            }

            if (identificationResultDto.UnidentifiedReasonSpecified)
            {
                return new IdentificationResult(IdentificationResultType.UnidentifiedReason, identificationResultDto.UnidentifiedReason.ToIdentificationError());
            }

            throw new ArgumentOutOfRangeException(nameof(identificationResultDto.Result), identificationResultDto.Result, null);
        }

        private static IdentificationResult IdentificationResultForDigipostOrPersonalIdentificationNumber(this V8.IdentificationResult identificationResultDto)
        {
            IdentificationResult identificationResult;

            switch (identificationResultDto.Result)
            {
                case V8.IdentificationResultCode.Digipost:
                    identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress, identificationResultDto.DigipostAddress + "");
                    break;
                case V8.IdentificationResultCode.Identified:
                    identificationResult = new IdentificationResult(IdentificationResultType.Personalias, !identificationResultDto.PersonAlias.Any() ? "" : identificationResultDto.PersonAlias.First() + "");
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
                Errorcode = error.ErrorCode,
                Errormessage = error.ErrorMessage,
                Errortype = error.ErrorType
            };
        }

        internal static SearchDetailsResult FromDataTransferObject(this V8.Recipients recipients)
        {
            return new SearchDetailsResult
            {
                PersonDetails = recipients.Recipient?.Select(
                    r => new SearchDetails
                    {
                        FirstName = r.Firstname,
                        MiddleName = r.Middlename,
                        LastName = r.Lastname,
                        DigipostAddress = r.DigipostAddress,
                        MobileNumber = r.MobileNumber,
                        OrganizationName = r.OrganisationName,
                        OrganizationNumber = r.OrganisationNumber,
                        SearchDetailsAddress = r.Address?.Select(a => new SearchDetailsAddress
                        {
                            Street = a.Street,
                            HouseNumber = a.HouseNumber,
                            HouseLetter = a.HouseLetter,
                            AdditionalAddressLine = a.AdditionalAddressline,
                            PostalCode = a.ZipCode,
                            City = a.City
                        })
                    })
            };
        }


        internal static ShareDocumentsRequestState FromDataTransferObject(this V8.ShareDocumentsRequestState dto)
        {
            return new ShareDocumentsRequestState(
                dto.SharedAtTimeSpecified ? dto.SharedAtTime : (DateTime?) null,
                dto.ExpiryTimeSpecified ? dto.ExpiryTime : (DateTime?) null,
                dto.WithdrawnTimeSpecified ? dto.WithdrawnTime : (DateTime?) null,
                dto.SharedDocuments.FromDataTransferObject(),
                dto.Link.FromDataTransferObject()
            );
        }

        private static IEnumerable<SharedDocument> FromDataTransferObject(this Collection<V8.SharedDocument> dto)
        {
            return dto.Select(document => new SharedDocument(
                document.DeliveryTime,
                document.Subject,
                document.FileType,
                Convert.ToInt32(document.FileSizeBytes),
                document.Origin.FromDataTransferObject(),
                document.Link.FromDataTransferObject()
            )).ToList();
        }

        private static IOrigin FromDataTransferObject(this V8.SharedDocumentOrigin dto)
        {
            if (dto.Organisation != null)
            {
                return new OrganisationOrigin(dto.Organisation.Name, dto.Organisation.OrganisationNumber);
            }

            return new PrivatePersonOrigin(dto.PrivatePerson.Name);
        }

        internal static SharedDocumentContent FromDataTransferObject(this V8.SharedDocumentContent dto)
        {
            return new SharedDocumentContent(dto.ContentType, new Uri(dto.Uri, UriKind.Absolute));
        }
    }
}
