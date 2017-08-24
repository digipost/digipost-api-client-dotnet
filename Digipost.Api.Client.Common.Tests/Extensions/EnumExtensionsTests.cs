using System;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        public class ToIdentificationChoiceType
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(IdentificationType));
                var enumValuesDto = Enum.GetValues(typeof(ItemChoiceType));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (IdentificationType) enumValue;

                    currentEnum.ToItemChoiceType();
                }
            }
        }

        public class ToAuthenticationLevel
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(AuthenticationLevel));
                var enumValuesDto = Enum.GetValues(typeof(authenticationlevel));

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
                var enumValuesDto = Enum.GetValues(typeof(sensitivitylevel));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (SensitivityLevel) enumValue;
                    currentEnum.ToSensitivityLevel();
                }
            }
        }

        public class ToPostType
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(PostType));
                var enumValuesDto = Enum.GetValues(typeof(posttype));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (PostType) enumValue;
                    currentEnum.ToPostType();
                }
            }
        }

        public class ToPrintColors
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(PrintColors));
                var enumValuesDto = Enum.GetValues(typeof(printcolors));

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
                var enumValuesDto = Enum.GetValues(typeof(nondeliverablehandling));

                var ignoredNondeliverableHandlingNotAvailableForPrivateSendersCount = 1;
                Assert.Equal(enumValues.Length, enumValuesDto.Length - ignoredNondeliverableHandlingNotAvailableForPrivateSendersCount);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (NondeliverableHandling) enumValue;
                    currentEnum.ToNondeliverablehandling();
                }
            }
        }

        public class ToCountryIdentifier
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValues = Enum.GetValues(typeof(CountryIdentifier));
                var enumValuesDto = Enum.GetValues(typeof(ItemChoiceType2));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValue in enumValues)
                {
                    var currentEnum = (CountryIdentifier) enumValue;
                    currentEnum.ToCountryIdentifier();
                }
            }
        }

        public class ToIdentificationResultType
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValuesDto = Enum.GetValues(typeof(ItemsChoiceType));
                var enumValues = Enum.GetValues(typeof(IdentificationResultType));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValueDto in enumValuesDto)
                {
                    var currentEnum = (ItemsChoiceType) enumValueDto;
                    currentEnum.ToIdentificationResultType();
                }
            }
        }

        public class ToDeliveryMethod
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValuesDto = Enum.GetValues(typeof(channel));
                var enumValues = Enum.GetValues(typeof(DeliveryMethod));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValueDto in enumValuesDto)
                {
                    var currentEnum = (channel) enumValueDto;
                    currentEnum.ToDeliveryMethod();
                }
            }
        }

        public class ToMessageStatus
        {
            [Fact]
            public void Converts_All_Enum_Values()
            {
                var enumValuesDto = Enum.GetValues(typeof(messagestatus));
                var enumValues = Enum.GetValues(typeof(MessageStatus));

                Assert.Equal(enumValues.Length, enumValuesDto.Length);

                foreach (var enumValueDto in enumValuesDto)
                {
                    var currentEnum = (messagestatus) enumValueDto;
                    currentEnum.ToMessageStatus();
                }
            }
        }
    }
}
