using System;
using System.Linq;
using System.Text.RegularExpressions;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using V8;

namespace Digipost.Api.Client.Common.Extensions
{
    internal static class EnumExtensions
    {

        internal static IdentificationError ToIdentificationError(this V8.Identification_Result_Code code)
        {
            switch (code)
            {
                case Identification_Result_Code.INVALID:
                    return IdentificationError.Invalid;
                case Identification_Result_Code.UNIDENTIFIED:
                    return IdentificationError.Unidentified;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

        internal static IdentificationError ToIdentificationError(this V8.Invalid_Reason code)
        {
            switch (code)
            {
                case Invalid_Reason.INVALID_ORGANISATION_NUMBER:
                    return IdentificationError.InvalidOrganisationNumber;
                case Invalid_Reason.INVALID_PERSONAL_IDENTIFICATION_NUMBER:
                    return IdentificationError.InvalidPersonalIdentificationNumber;
                case Invalid_Reason.UNKNOWN:
                    return IdentificationError.Unknown;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

        internal static IdentificationError ToIdentificationError(this V8.Unidentified_Reason code)
        {
            switch (code)
            {
                case Unidentified_Reason.NOT_FOUND:
                    return IdentificationError.Unknown; // <- See documentation on IdentificationError for explaination for `Unknown` here
                case Unidentified_Reason.MULTIPLE_MATCHES:
                    return IdentificationError.MultipleMatches;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

        internal static V8.Authentication_Level ToAuthenticationLevel(this AuthenticationLevel authenticationLevel)
        {
            switch (authenticationLevel)
            {
                case AuthenticationLevel.Password:
                    return V8.Authentication_Level.PASSWORD;
                case AuthenticationLevel.TwoFactor:
                    return V8.Authentication_Level.TWO_FACTOR;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationLevel), authenticationLevel, null);
            }
        }

        internal static V8.Sensitivity_Level ToSensitivityLevel(this SensitivityLevel sensitivityLevel)
        {
            switch (sensitivityLevel)
            {
                case SensitivityLevel.Normal:
                    return V8.Sensitivity_Level.NORMAL;
                case SensitivityLevel.Sensitive:
                    return V8.Sensitivity_Level.SENSITIVE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensitivityLevel), sensitivityLevel, null);
            }
        }

        internal static V8.Print_Colors ToPrintColors(this PrintColors printColors)
        {
            switch (printColors)
            {
                case PrintColors.Monochrome:
                    return V8.Print_Colors.MONOCHROME;
                case PrintColors.Colors:
                    return V8.Print_Colors.COLORS;
                default:
                    throw new ArgumentOutOfRangeException(nameof(printColors), printColors, null);
            }
        }

        internal static V8.Nondeliverable_Handling ToNondeliverablehandling(this NondeliverableHandling nondeliverableHandling)
        {
            switch (nondeliverableHandling)
            {
                case NondeliverableHandling.ReturnToSender:
                    return V8.Nondeliverable_Handling.RETURN_TO_SENDER;
                case NondeliverableHandling.Shred:
                    return V8.Nondeliverable_Handling.SHRED;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nondeliverableHandling), nondeliverableHandling, null);
            }
        }


        internal static DeliveryMethod ToDeliveryMethod(this V8.Channel deliveryMethod)
        {
            switch (deliveryMethod)
            {
                case V8.Channel.PRINT:
                    return DeliveryMethod.Print;
                case V8.Channel.DIGIPOST:
                    return DeliveryMethod.Digipost;
                case V8.Channel.PEPPOL:
                    return DeliveryMethod.PEPPOL;
                case V8.Channel.EPOST:
                    return DeliveryMethod.EPOST;
                case V8.Channel.PENDING:
                    return DeliveryMethod.PENDING;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null);
            }
        }

        internal static HashAlgoritm ToHashAlgoritm(this V8.Hash_Algorithm hashAlgorithm)
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

        internal static MessageStatus ToMessageStatus(this V8.Message_Status messagestatus)
        {
            switch (messagestatus)
            {
                case Message_Status.NOT_COMPLETE:
                    return MessageStatus.NotComplete;
                case Message_Status.COMPLETE:
                    return MessageStatus.Complete;
                case Message_Status.DELIVERED:
                    return MessageStatus.Delivered;
                case Message_Status.DELIVERED_TO_PRINT:
                    return MessageStatus.DeliveredToPrint;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messagestatus), messagestatus, null);
            }
        }

        internal static AuthenticationLevel ToAuthenticationLevel(this V8.Authentication_Level authenticationlevel)
        {
            switch (authenticationlevel)
            {
                case Authentication_Level.PASSWORD:
                    return AuthenticationLevel.Password;
                case Authentication_Level.TWO_FACTOR:
                    return AuthenticationLevel.TwoFactor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationlevel), authenticationlevel, null);
            }
        }

        internal static SensitivityLevel ToSensitivityLevel(this V8.Sensitivity_Level sensitivitylevel)
        {
            switch (sensitivitylevel)
            {
                case Sensitivity_Level.NORMAL:
                    return SensitivityLevel.Normal;
                case Sensitivity_Level.SENSITIVE:
                    return SensitivityLevel.Sensitive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensitivitylevel), sensitivitylevel, null);
            }
        }

        internal static DocumentEventType ToEventType(this V8.Event_Type eventType)
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
