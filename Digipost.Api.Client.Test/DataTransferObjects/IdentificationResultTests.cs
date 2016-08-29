using System;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Xunit;

namespace Digipost.Api.Client.Test.DataTransferObjects
{
    public class IdentificationResultTests
    {
        public class ConstructorMethod : IdentificationResultTests
        {
            [Fact]
            public void SuccessfulIdentificationResultTypeSetsResult()
            {
                var enumValues = Enum.GetValues(typeof (IdentificationResultType)).Cast<IdentificationResultType>();

                foreach (var value in enumValues)
                {
                    var failedIdentificationResultType = value == IdentificationResultType.InvalidReason || value == IdentificationResultType.UnidentifiedReason;

                    if (!failedIdentificationResultType)
                    {
                        //Arrange
                        const string result = "Digipost-address, personalias or empty";

                        var identificationResult = new IdentificationResult(value, result);

                        //Act

                        //Assert
                        Assert.Equal(result, identificationResult.Data);
                        Assert.Null(identificationResult.Error);
                    }
                }
            }

            [Fact]
            public void SetsIdentificationResultType()
            {
                //Arrange
                const string digipostAddress = "ola.nordmann#2433B";
                var identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);

                //Act

                //Assert
                Assert.Equal(identificationResult.ResultType, IdentificationResultType.DigipostAddress);
            }

            [Fact]
            public void IdentificationResultCodeErrorSetsIdentificationErrorNotResult()
            {
                var enumValues = Enum.GetValues(typeof (IdentificationResultCode)).Cast<IdentificationResultCode>();

                foreach (var value in enumValues)
                {
                    var successfulIdentificationResultCode = value == IdentificationResultCode.Digipost ||
                                                             value == IdentificationResultCode.Identified;

                    if (!successfulIdentificationResultCode)
                    {
                        //Arrange
                        var identificationResult = new IdentificationResult(IdentificationResultType.InvalidReason, value.ToString());

                        //Act

                        //Assert
                        Assert.Equal(identificationResult.Error.ToString(), value.ToString());
                        Assert.Null(identificationResult.Data);
                    }
                }
            }

            [Fact]
            public void InvalidReasonErrorSetsIdentificationErrorNotResult()
            {
                var enumValues = Enum.GetValues(typeof (InvalidReason)).Cast<InvalidReason>();

                foreach (var value in enumValues)
                {
                    //Arrange
                    var identificationResult = new IdentificationResult(IdentificationResultType.InvalidReason, value.ToString());

                    //Act

                    //Assert
                    Assert.Equal(identificationResult.Error.ToString(), value.ToString());
                    Assert.Null(identificationResult.Data);
                }
            }
        }
    }
}