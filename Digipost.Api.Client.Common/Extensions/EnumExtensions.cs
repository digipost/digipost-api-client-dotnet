using System;
using Digipost.Api.Client.Common.Enums;
using V7;

namespace Digipost.Api.Client.Common.Extensions
{
    internal static class EnumExtensions
    {

        public static V7.Authentication_Level ToAuthenticationLevel(this AuthenticationLevel authenticationLevel)
        {
            switch (authenticationLevel)
            {
                case AuthenticationLevel.Password:
                    return V7.Authentication_Level.PASSWORD;
                case AuthenticationLevel.TwoFactor:
                    return V7.Authentication_Level.TWO_FACTOR;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authenticationLevel), authenticationLevel, null);
            }
        }

        public static V7.Sensitivity_Level ToSensitivityLevel(this SensitivityLevel sensitivityLevel)
        {
            switch (sensitivityLevel)
            {
                case SensitivityLevel.Normal:
                    return V7.Sensitivity_Level.NORMAL;
                case SensitivityLevel.Sensitive:
                    return V7.Sensitivity_Level.SENSITIVE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensitivityLevel), sensitivityLevel, null);
            }
        }

        public static V7.Print_Colors ToPrintColors(this PrintColors printColors)
        {
            switch (printColors)
            {
                case PrintColors.Monochrome:
                    return V7.Print_Colors.MONOCHROME;
                case PrintColors.Colors:
                    return V7.Print_Colors.COLORS;
                default:
                    throw new ArgumentOutOfRangeException(nameof(printColors), printColors, null);
            }
        }

        public static V7.Nondeliverable_Handling ToNondeliverablehandling(this NondeliverableHandling nondeliverableHandling)
        {
            switch (nondeliverableHandling)
            {
                case NondeliverableHandling.ReturnToSender:
                    return V7.Nondeliverable_Handling.RETURN_TO_SENDER;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nondeliverableHandling), nondeliverableHandling, null);
            }
        }


        public static DeliveryMethod ToDeliveryMethod(this V7.Channel deliveryMethod)
        {
            switch (deliveryMethod)
            {
                case V7.Channel.PRINT:
                    return DeliveryMethod.Print;
                case V7.Channel.DIGIPOST:
                    return DeliveryMethod.Digipost;
                case V7.Channel.PEPPOL:
                    return DeliveryMethod.PEPPOL;
                case V7.Channel.EPOST:
                    return DeliveryMethod.EPOST;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null);
            }
        }

        public static MessageStatus ToMessageStatus(this V7.Message_Status messagestatus)
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

        public static AuthenticationLevel ToAuthenticationLevel(this V7.Authentication_Level authenticationlevel)
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

        public static SensitivityLevel ToSensitivityLevel(this V7.Sensitivity_Level sensitivitylevel)
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
    }
}
