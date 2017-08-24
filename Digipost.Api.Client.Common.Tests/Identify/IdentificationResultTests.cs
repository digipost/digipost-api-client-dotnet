using System;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Identify;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Identify
{
    public class IdentificationResultTests
    {
        public class ConstructorMethod_first_parameter_triggers_data_or_error : IdentificationResultTests
        {
            [Fact]
            public void Successful_IdentificationResultType_sets_data_not_error()
            {
                //Arrange
                var actualEnumValues = Enum.GetValues(typeof(ItemsChoiceType)).Cast<ItemsChoiceType>().ToArray();
                var expectedEnumValues = new[]
                {
                    ItemsChoiceType.digipostaddress,
                    ItemsChoiceType.invalidreason,
                    ItemsChoiceType.personalias,
                    ItemsChoiceType.unidentifiedreason
                };

                Assert.NotStrictEqual(expectedEnumValues, actualEnumValues);

                foreach (var enumValue in actualEnumValues)
                {
                    var isSuccessfulItemsChoiceType = enumValue == ItemsChoiceType.digipostaddress || enumValue == ItemsChoiceType.personalias;

                    //Act
                    var resultCode = isSuccessfulItemsChoiceType ? "Digipost-address, personalias or empty" : "UNIDENTIFIED";
                    var identificationResult = new IdentificationResult(enumValue.ToIdentificationResultType(), resultCode);

                    if (isSuccessfulItemsChoiceType)
                    {
                        //Assert
                        Assert.Equal(resultCode, identificationResult.Data);
                        Assert.Null(identificationResult.Error);
                    }
                    else
                    {
                        //Assert
                        Assert.NotNull(identificationResult.Error);
                        Assert.Null(identificationResult.Data);
                    }
                }
            }
        }

        public class ConstructorMethod_second_parameter_can_handle_all_identification_error
        {
            [Fact]
            public void Handles_identificationresultcode()
            {
                var actualidentificationresultcodes = Enum.GetValues(typeof(identificationresultcode)).Cast<identificationresultcode>().ToArray();
                var expectedidentificationresultcodes = new[]
                {
                    identificationresultcode.DIGIPOST,
                    identificationresultcode.IDENTIFIED,
                    identificationresultcode.INVALID,
                    identificationresultcode.UNIDENTIFIED
                };

                Assert.NotStrictEqual(expectedidentificationresultcodes, actualidentificationresultcodes);

                foreach (var resultcode in actualidentificationresultcodes)
                {
                    var isFailedIdentificationresultcodes = resultcode == identificationresultcode.INVALID || resultcode == identificationresultcode.UNIDENTIFIED;

                    //Arrange
                    var nonEssentialFirstParameter = IdentificationResultType.InvalidReason;

                    if (isFailedIdentificationresultcodes)
                    {
                        //Act
                        var identificationResult = new IdentificationResult(nonEssentialFirstParameter, resultcode.ToString());

                        //Assert
                        Assert.NotNull(identificationResult.Error.ToString());
                        Assert.Null(identificationResult.Data);
                    }
                }
            }

            [Fact]
            public void Handles_invalidreason()
            {
                var actualInvalidreasons = Enum.GetValues(typeof(invalidreason)).Cast<invalidreason>().ToArray();
                var expectedInvalidreasons = new[]
                {
                    invalidreason.INVALID_ORGANISATION_NUMBER,
                    invalidreason.INVALID_PERSONAL_IDENTIFICATION_NUMBER,
                    invalidreason.UNKNOWN
                };

                Assert.NotStrictEqual(expectedInvalidreasons, actualInvalidreasons);

                foreach (var reason in actualInvalidreasons)
                {
                    //Arrange
                    var nonEssentialFirstParameter = IdentificationResultType.InvalidReason;

                    //Act
                    var identificationResult = new IdentificationResult(nonEssentialFirstParameter, reason.ToString());

                    //Assert
                    Assert.NotNull(identificationResult.Error.ToString());
                    Assert.Null(identificationResult.Data);
                }
            }

            [Fact]
            public void Handles_unidentifiedreason()
            {
                var actualUnidentifiedreasons = Enum.GetValues(typeof(unidentifiedreason)).Cast<unidentifiedreason>().ToArray();
                var expectedUnidentifiedreasons = new[]
                {
                    unidentifiedreason.MULTIPLE_MATCHES,
                    unidentifiedreason.NOT_FOUND
                };

                Assert.NotStrictEqual(expectedUnidentifiedreasons, actualUnidentifiedreasons);

                foreach (var reason in actualUnidentifiedreasons)
                {
                    //Arrange
                    var nonEssentialFirstParameter = IdentificationResultType.InvalidReason;

                    //Act
                    var identificationResult = new IdentificationResult(nonEssentialFirstParameter, reason.ToString());

                    //Assert
                    Assert.NotNull(identificationResult.Error.ToString());
                    Assert.Null(identificationResult.Data);
                }
            }
        }
    }
}