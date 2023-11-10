using System;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using V8 = Digipost.Api.Client.Common.Generated.V8;
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
                var enumValuesDto = Enum.GetValues(typeof(V8.AuthenticationLevel));

                var ignoredIdPortenAlternativesNotAvailableForPrivateSendersCount = 2;
                Assert.Equal(enumValues.Length, enumValuesDto.Length - ignoredIdPortenAlternativesNotAvailableForPrivateSendersCount);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (AuthenticationLevel) enumValue;
                    currentEnum.ToAuthenticationLevel();
                }
            }
        }

        public class ToEventType
        {
            [Fact]
            public void Convert_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(DocumentEventType));
                var enumValuesDto = Enum.GetValues(typeof(V8.EventType));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValue in enumValuesDto)
                {
                    var currentEnum = (V8.EventType) enumValue;
                    currentEnum.ToEventType();
                }
            }
        }

        public class ToSensitivityLevel
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(SensitivityLevel));
                var enumValuesDto = Enum.GetValues(typeof(V8.SensitivityLevel));

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
                var enumValuesDto = Enum.GetValues(typeof(V8.PrintColors));

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
                var enumValuesDto = Enum.GetValues(typeof(V8.NondeliverableHandling));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

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
                var enumValuesDto = Enum.GetValues(typeof(V8.MessageStatus));
                var enumValues = Enum.GetValues(typeof(MessageStatus));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValueDto in enumValuesDto)
                {
                    var currentEnum = (V8.MessageStatus) enumValueDto;
                    currentEnum.ToMessageStatus();
                }
            }
        }
    }
}
