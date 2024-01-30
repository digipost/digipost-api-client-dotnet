using System;
using System.Linq;
using System.Text.RegularExpressions;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using V8 = Digipost.Api.Client.Common.Generated.V8;

namespace Digipost.Api.Client.Common.Extensions
{
    internal static class EnumExtensions
    {


        internal static IdentificationError ToIdentificationError(this V8.IdentificationResultCode code)
        {
            switch (code)
            {
                case V8.IdentificationResultCode.Invalid:
                    return IdentificationError.Invalid;
                case V8.IdentificationResultCode.Unidentified:
                    return IdentificationError.Unidentified;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

        internal static IdentificationError ToIdentificationError(this V8.InvalidReason code)
        {
            switch (code)
            {
                case V8.InvalidReason.InvalidOrganisationNumber:
                    return IdentificationError.InvalidOrganisationNumber;
                case V8.InvalidReason.InvalidPersonalIdentificationNumber:
                    return IdentificationError.InvalidPersonalIdentificationNumber;
                case V8.InvalidReason.Unknown:
                    return IdentificationError.Unknown;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

        internal static IdentificationError ToIdentificationError(this V8.UnidentifiedReason code)
        {
            switch (code)
            {
                case V8.UnidentifiedReason.NotFound:
                    return IdentificationError.Unknown; // <- See documentation on IdentificationError for explaination for `Unknown` here
                case V8.UnidentifiedReason.MultipleMatches:
                    return IdentificationError.MultipleMatches;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

        internal static V8.AuthenticationLevel ToAuthenticationLevel(this AuthenticationLevel authenticationLevel)
        {
            switch (authenticationLevel)
            {
                case AuthenticationLevel.Password:
                    return V8.AuthenticationLevel.Password;
                case AuthenticationLevel.TwoFactor:
                    return V8.AuthenticationLevel.TwoFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationLevel), authenticationLevel, null);
            }
        }

        internal static V8.SensitivityLevel ToSensitivityLevel(this SensitivityLevel sensitivityLevel)
        {
            switch (sensitivityLevel)
            {
                case SensitivityLevel.Normal:
                    return V8.SensitivityLevel.Normal;
                case SensitivityLevel.Sensitive:
                    return V8.SensitivityLevel.Sensitive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensitivityLevel), sensitivityLevel, null);
            }
        }

        internal static V8.PrintColors ToPrintColors(this PrintColors printColors)
        {
            switch (printColors)
            {
                case PrintColors.Monochrome:
                    return V8.PrintColors.Monochrome;
                case PrintColors.Colors:
                    return V8.PrintColors.Colors;
                default:
                    throw new ArgumentOutOfRangeException(nameof(printColors), printColors, null);
            }
        }

        internal static V8.NondeliverableHandling ToNondeliverablehandling(this NondeliverableHandling nondeliverableHandling)
        {
            switch (nondeliverableHandling)
            {
                case NondeliverableHandling.ReturnToSender:
                    return V8.NondeliverableHandling.ReturnToSender;
                case NondeliverableHandling.Shred:
                    return V8.NondeliverableHandling.Shred;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nondeliverableHandling), nondeliverableHandling, null);
            }
        }


        internal static DeliveryMethod ToDeliveryMethod(this V8.Channel deliveryMethod)
        {
            switch (deliveryMethod)
            {
                case V8.Channel.Print:
                    return DeliveryMethod.Print;
                case V8.Channel.Digipost:
                    return DeliveryMethod.Digipost;
                case V8.Channel.Peppol:
                    return DeliveryMethod.PEPPOL;
                case V8.Channel.Epost:
                    return DeliveryMethod.EPOST;
                case V8.Channel.Pending:
                    return DeliveryMethod.PENDING;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null);
            }
        }

        internal static HashAlgoritm ToHashAlgoritm(this V8.HashAlgorithm hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case V8.HashAlgorithm.None:
                    return HashAlgoritm.NONE;
                case V8.HashAlgorithm.Md5:
                    return HashAlgoritm.MD5;
                case V8.HashAlgorithm.Sha256:
                    return HashAlgoritm.SHA256;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static MessageStatus ToMessageStatus(this V8.MessageStatus messagestatus)
        {
            switch (messagestatus)
            {
                case V8.MessageStatus.NotComplete:
                    return MessageStatus.NotComplete;
                case V8.MessageStatus.Complete:
                    return MessageStatus.Complete;
                case V8.MessageStatus.Delivered:
                    return MessageStatus.Delivered;
                case V8.MessageStatus.DeliveredToPrint:
                    return MessageStatus.DeliveredToPrint;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messagestatus), messagestatus, null);
            }
        }

        internal static AuthenticationLevel ToAuthenticationLevel(this V8.AuthenticationLevel authenticationlevel)
        {
            switch (authenticationlevel)
            {
                case V8.AuthenticationLevel.Password:
                    return AuthenticationLevel.Password;
                case V8.AuthenticationLevel.TwoFactor:
                    return AuthenticationLevel.TwoFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationlevel), authenticationlevel, null);
            }
        }

        internal static SensitivityLevel ToSensitivityLevel(this V8.SensitivityLevel sensitivitylevel)
        {
            switch (sensitivitylevel)
            {
                case V8.SensitivityLevel.Normal:
                    return SensitivityLevel.Normal;
                case V8.SensitivityLevel.Sensitive:
                    return SensitivityLevel.Sensitive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensitivitylevel), sensitivitylevel, null);
            }
        }

        internal static DocumentEventType ToEventType(this V8.EventType eventType)
        {
            return MapEnum<DocumentEventType>(eventType);
        }

        private static TTargetEnum MapEnum<TTargetEnum>(Enum sourceEnum) where TTargetEnum : Enum
        {
            Type targetEnumType = typeof(TTargetEnum);

            string sourceEnumName = sourceEnum.ToString();
            string[] targetEnumNames = Enum.GetNames(targetEnumType);

            string mappedValueName = targetEnumNames.FirstOrDefault(
                targetName => string.Equals(
                    RemoveUnderscores(targetName),
                    RemoveUnderscores(sourceEnumName),
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (!string.IsNullOrEmpty(mappedValueName))
            {
                object mappedValue = Enum.Parse(targetEnumType, mappedValueName);
                return (TTargetEnum)mappedValue;
            }

            throw new ArgumentException($"Enum value {sourceEnum} not found in {targetEnumType.Name} enum.");
        }
        private static string RemoveUnderscores(string input)
        {
            return Regex.Replace(input, "_", string.Empty);
        }
    }
}
