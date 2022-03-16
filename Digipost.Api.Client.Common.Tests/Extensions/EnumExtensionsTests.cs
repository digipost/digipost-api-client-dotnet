using System;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using V8;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Extensions
{
    public class EnumExtensionsTests
    {

        public class ToAuthenticationLevel
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(AuthenticationLevel));
                var enumValuesDto = Enum.GetValues(typeof(V8.Authentication_Level));

                var ignoredIdPortenAlternativesNotAvailableForPrivateSendersCount = 2;
                Assert.Equal(enumValues.Length, enumValuesDto.Length - ignoredIdPortenAlternativesNotAvailableForPrivateSendersCount);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (AuthenticationLevel) enumValue;
                    currentEnum.ToAuthenticationLevel();
                }
            }
        }

        public class ToSensitivityLevel
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(SensitivityLevel));
                var enumValuesDto = Enum.GetValues(typeof(V8.Sensitivity_Level));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (SensitivityLevel) enumValue;
                    currentEnum.ToSensitivityLevel();
                }
            }
        }

        public class ToPrintColors
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(PrintColors));
                var enumValuesDto = Enum.GetValues(typeof(V8.Print_Colors));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (PrintColors) enumValue;
                    currentEnum.ToPrintColors();
                }
            }
        }

        public class ToNondeliverableHandling
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(NondeliverableHandling));
                var enumValuesDto = Enum.GetValues(typeof(V8.Nondeliverable_Handling));

                var ignoredNondeliverableHandlingNotAvailableForPrivateSendersCount = 1;
                Assert.Equal(enumValues.Length, enumValuesDto.Length - ignoredNondeliverableHandlingNotAvailableForPrivateSendersCount);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (NondeliverableHandling) enumValue;
                    currentEnum.ToNondeliverablehandling();
                }
            }
        }


        public class ToDeliveryMethod
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValuesDto = Enum.GetValues(typeof(V8.Channel));
                var enumValues = Enum.GetValues(typeof(DeliveryMethod));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValueDto in enumValuesDto)
                {
                    var currentEnum = (V8.Channel) enumValueDto;
                    currentEnum.ToDeliveryMethod();
                }
            }
        }

        public class ToMessageStatus
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValuesDto = Enum.GetValues(typeof(V8.Message_Status));
                var enumValues = Enum.GetValues(typeof(MessageStatus));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValueDto in enumValuesDto)
                {
                    var currentEnum = (V8.Message_Status) enumValueDto;
                    currentEnum.ToMessageStatus();
                }
            }
        }
    }
}
