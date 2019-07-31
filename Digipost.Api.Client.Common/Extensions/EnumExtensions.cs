using System;
using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Extensions
{
    internal static class EnumExtensions
    {
        public static ItemChoiceType ToItemChoiceType(this IdentificationType identificationType)
        {
            switch (identificationType)
            {
                case IdentificationType.DigipostAddress:
                    return ItemChoiceType.digipostaddress;
                case IdentificationType.PersonalIdentificationNumber:
                    return ItemChoiceType.personalidentificationnumber;
                case IdentificationType.OrganizationNumber:
                    return ItemChoiceType.organisationnumber;
                case IdentificationType.NameAndAddress:
                    return ItemChoiceType.nameandaddress;
                default:
                    throw new ArgumentOutOfRangeException(nameof(identificationType), identificationType, null);
            }
        }

        public static ItemChoiceType1 ToItemChoiceType1(this IdentificationType identificationType)
        {
            switch (identificationType)
            {
                case IdentificationType.DigipostAddress:
                    return ItemChoiceType1.digipostaddress;
                case IdentificationType.PersonalIdentificationNumber:
                    return ItemChoiceType1.personalidentificationnumber;
                case IdentificationType.OrganizationNumber:
                    return ItemChoiceType1.organisationnumber;
                case IdentificationType.NameAndAddress:
                    return ItemChoiceType1.nameandaddress;
                default:
                    throw new ArgumentOutOfRangeException(nameof(identificationType), identificationType, null);
            }
        }

        public static authenticationlevel ToAuthenticationLevel(this AuthenticationLevel authenticationLevel)
        {
            switch (authenticationLevel)
            {
                case AuthenticationLevel.Password:
                    return authenticationlevel.PASSWORD;
                case AuthenticationLevel.TwoFactor:
                    return authenticationlevel.TWO_FACTOR;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationLevel), authenticationLevel, null);
            }
        }

        public static sensitivitylevel ToSensitivityLevel(this SensitivityLevel sensitivityLevel)
        {
            switch (sensitivityLevel)
            {
                case SensitivityLevel.Normal:
                    return sensitivitylevel.NORMAL;
                case SensitivityLevel.Sensitive:
                    return sensitivitylevel.SENSITIVE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensitivityLevel), sensitivityLevel, null);
            }
        }

        public static posttype ToPostType(this PostType postType)
        {
            switch (postType)
            {
                case PostType.A:
                    return posttype.A;
                case PostType.B:
                    return posttype.B;
                default:
                    throw new ArgumentOutOfRangeException(nameof(postType), postType, null);
            }
        }

        public static printcolors ToPrintColors(this PrintColors printColors)
        {
            switch (printColors)
            {
                case PrintColors.Monochrome:
                    return printcolors.MONOCHROME;
                case PrintColors.Colors:
                    return printcolors.COLORS;
                default:
                    throw new ArgumentOutOfRangeException(nameof(printColors), printColors, null);
            }
        }

        public static nondeliverablehandling ToNondeliverablehandling(this NondeliverableHandling nondeliverableHandling)
        {
            switch (nondeliverableHandling)
            {
                case NondeliverableHandling.ReturnToSender:
                    return nondeliverablehandling.RETURN_TO_SENDER;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nondeliverableHandling), nondeliverableHandling, null);
            }
        }

        public static ItemChoiceType2 ToCountryIdentifier(this CountryIdentifier countryIdentifier)
        {
            switch (countryIdentifier)
            {
                case CountryIdentifier.Country:
                    return ItemChoiceType2.country;
                case CountryIdentifier.Countrycode:
                    return ItemChoiceType2.countrycode;
                default:
                    throw new ArgumentOutOfRangeException(nameof(countryIdentifier), countryIdentifier, null);
            }
        }

        public static IdentificationResultType ToIdentificationResultType(this ItemsChoiceType itemsChoiceType)
        {
            switch (itemsChoiceType)
            {
                case ItemsChoiceType.digipostaddress:
                    return IdentificationResultType.DigipostAddress;
                case ItemsChoiceType.invalidreason:
                    return IdentificationResultType.InvalidReason;
                case ItemsChoiceType.personalias:
                    return IdentificationResultType.Personalias;
                case ItemsChoiceType.unidentifiedreason:
                    return IdentificationResultType.UnidentifiedReason;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemsChoiceType), itemsChoiceType, null);
            }
        }

        public static DeliveryMethod ToDeliveryMethod(this channel deliveryMethod)
        {
            switch (deliveryMethod)
            {
                case channel.PRINT:
                    return DeliveryMethod.Print;
                case channel.DIGIPOST:
                    return DeliveryMethod.Digipost;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null);
            }
        }

        public static MessageStatus ToMessageStatus(this messagestatus messagestatus)
        {
            switch (messagestatus)
            {
                case messagestatus.NOT_COMPLETE:
                    return MessageStatus.NotComplete;
                case messagestatus.COMPLETE:
                    return MessageStatus.Complete;
                case messagestatus.DELIVERED:
                    return MessageStatus.Delivered;
                case messagestatus.DELIVERED_TO_PRINT:
                    return MessageStatus.DeliveredToPrint;
                case messagestatus.DELIVERED_WITH_PRINT_FALLBACK:
                    return MessageStatus.DeliveredWithPrintFallback;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messagestatus), messagestatus, null);
            }
        }

        public static AuthenticationLevel ToAuthenticationLevel(this authenticationlevel authenticationlevel)
        {
            switch (authenticationlevel)
            {
                case authenticationlevel.PASSWORD:
                    return AuthenticationLevel.Password;
                case authenticationlevel.TWO_FACTOR:
                    return AuthenticationLevel.TwoFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationlevel), authenticationlevel, null);
            }
        }

        public static SensitivityLevel ToSensitivityLevel(this sensitivitylevel sensitivitylevel)
        {
            switch (sensitivitylevel)
            {
                case sensitivitylevel.NORMAL:
                    return SensitivityLevel.Normal;
                case sensitivitylevel.SENSITIVE:
                    return SensitivityLevel.Sensitive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensitivitylevel), sensitivitylevel, null);
            }
        }
    }
}
