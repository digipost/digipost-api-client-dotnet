using System;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Identify;
using V8 = Digipost.Api.Client.Common.Generated.V8;
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
                var actualidentificationresultcodes = Enum.GetValues(typeof(V8.IdentificationResultCode)).Cast<V8.IdentificationResultCode>().ToArray();
                var expectedidentificationresultcodes = new[]
                {
                    V8.IdentificationResultCode.Digipost,
                    V8.IdentificationResultCode.Identified,
                    V8.IdentificationResultCode.Invalid,
                    V8.IdentificationResultCode.Unidentified
                };

                Assert.NotStrictEqual(expectedidentificationresultcodes, actualidentificationresultcodes);

                foreach (var resultcode in actualidentificationresultcodes)
                {
                    var isFailedIdentificationresultcodes = resultcode == V8.IdentificationResultCode.Invalid || resultcode == V8.IdentificationResultCode.Unidentified;

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
                var actualInvalidreasons = Enum.GetValues(typeof(V8.InvalidReason)).Cast<V8.InvalidReason>().ToArray();
                var expectedInvalidreasons = new[]
                {
                    V8.InvalidReason.InvalidOrganisationNumber,
                    V8.InvalidReason.InvalidPersonalIdentificationNumber,
                    V8.InvalidReason.Unknown
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
                var actualUnidentifiedreasons = Enum.GetValues(typeof(V8.UnidentifiedReason)).Cast<V8.UnidentifiedReason>().ToArray();
                var expectedUnidentifiedreasons = new[]
                {
                    V8.UnidentifiedReason.MultipleMatches,
                    V8.UnidentifiedReason.NotFound
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
