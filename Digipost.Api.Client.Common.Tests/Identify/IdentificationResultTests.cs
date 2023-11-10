using System;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Identify;
using V8;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Identify
{
    public class IdentificationResultTests
    {

        public class ConstructorMethod_second_parameter_can_handle_all_identification_error
        {
            [Fact]
            public void Handles_identificationresultcode()
            {
                var actualidentificationresultcodes = Enum.GetValues(typeof(V8.Identification_Result_Code)).Cast<V8.Identification_Result_Code>().ToArray();
                var expectedidentificationresultcodes = new[]
                {
                    V8.Identification_Result_Code.DIGIPOST,
                    V8.Identification_Result_Code.IDENTIFIED,
                    V8.Identification_Result_Code.INVALID,
                    V8.Identification_Result_Code.UNIDENTIFIED
                };

                Assert.NotStrictEqual(expectedidentificationresultcodes, actualidentificationresultcodes);

                foreach (var resultcode in actualidentificationresultcodes)
                {
                    var isFailedIdentificationresultcodes = resultcode == V8.Identification_Result_Code.INVALID || resultcode == V8.Identification_Result_Code.UNIDENTIFIED;

                    //Arrange
                    var nonEssentialFirstParameter = IdentificationResultType.InvalidReason;

                    if (isFailedIdentificationresultcodes)
                    {
                        //Act
                        var identificationResult = new IdentificationResult(nonEssentialFirstParameter, resultcode.ToIdentificationError());

                        //Assert
                        Assert.NotNull(identificationResult.Error.ToString());
                        Assert.Null(identificationResult.Data);
                    }
                }
            }

            [Fact]
            public void Handles_invalidreason()
            {
                var actualInvalidreasons = Enum.GetValues(typeof(V8.Invalid_Reason)).Cast<V8.Invalid_Reason>().ToArray();
                var expectedInvalidreasons = new[]
                {
                    V8.Invalid_Reason.INVALID_ORGANISATION_NUMBER,
                    V8.Invalid_Reason.INVALID_PERSONAL_IDENTIFICATION_NUMBER,
                    V8.Invalid_Reason.UNKNOWN
                };

                Assert.NotStrictEqual(expectedInvalidreasons, actualInvalidreasons);

                foreach (var reason in actualInvalidreasons)
                {
                    //Arrange
                    var nonEssentialFirstParameter = IdentificationResultType.InvalidReason;

                    //Act
                    var identificationResult = new IdentificationResult(nonEssentialFirstParameter, reason.ToIdentificationError());

                    //Assert
                    Assert.NotNull(identificationResult.Error.ToString());
                    Assert.Null(identificationResult.Data);
                }
            }

            [Fact]
            public void Handles_unidentifiedreason()
            {
                var actualUnidentifiedreasons = Enum.GetValues(typeof(V8.Unidentified_Reason)).Cast<V8.Unidentified_Reason>().ToArray();
                var expectedUnidentifiedreasons = new[]
                {
                    V8.Unidentified_Reason.MULTIPLE_MATCHES,
                    V8.Unidentified_Reason.NOT_FOUND
                };

                Assert.NotStrictEqual(expectedUnidentifiedreasons, actualUnidentifiedreasons);

                foreach (var reason in actualUnidentifiedreasons)
                {
                    //Arrange
                    var nonEssentialFirstParameter = IdentificationResultType.InvalidReason;

                    //Act
                    var identificationResult = new IdentificationResult(nonEssentialFirstParameter, reason.ToIdentificationError());

                    //Assert
                    Assert.NotNull(identificationResult.Error.ToString());
                    Assert.Null(identificationResult.Data);
                }
            }
        }
    }
}
