using System;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class IdentificationResultTests
    {
        [TestClass]
        public class ConstructorMethod : IdentificationTests
        {
            [TestMethod]
            public void SuccessfulIntentificationResultTypeSetsResult()
            {
                var enumValues = Enum.GetValues(typeof(IdentificationResultType)).Cast<IdentificationResultType>();

                foreach (var value in enumValues)
                {

                    bool failedIdentificationResultType = (value == IdentificationResultType.InvalidReason ||
                                                           value == IdentificationResultType.UnidentifiedReason);

                    if (!failedIdentificationResultType)
                    {
                        //Arrange
                        IdentificationResult identificationResult = new IdentificationResult(value, "ola.nordmann#2433B");

                        //Act

                        //Assert
                        Assert.AreEqual("ola.nordmann#2433B", identificationResult.Data);
                        Assert.IsNull(identificationResult.Error);
                    }
                }
            }

            [TestMethod]
            public void SetsIdentificationResultType()
            {
                //Arrange
                IdentificationResult identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress, "ola.nordmann#2433B");

                //Act

                //Assert
                Assert.AreEqual(identificationResult.ResultType, IdentificationResultType.DigipostAddress);
            }

            [TestMethod]
            public void IdentificationResultCodeErrorSetsIdentificationErrorNotResult()
            {
                var enumValues = Enum.GetValues(typeof(IdentificationResultCode)).Cast<IdentificationResultCode>();

                foreach (var value in enumValues)
                {
                    bool successfulIdentificationResultCode = (value == IdentificationResultCode.Digipost ||
                                                 value == IdentificationResultCode.Identified);

                    if (!successfulIdentificationResultCode)
                    {
                        //Arrange
                        IdentificationResult identificationResult = new IdentificationResult(IdentificationResultType.InvalidReason, value.ToString());

                        //Act

                        //Assert
                        Assert.AreEqual(identificationResult.Error.ToString(), value.ToString());
                        Assert.IsNull(identificationResult.Data);

                    }
                }
            }

            [TestMethod]
            public void InvalidReasonErrorSetsIdentificationErrorNotResult()
            {
                var enumValues = Enum.GetValues(typeof(InvalidReason)).Cast<InvalidReason>();

                foreach (var value in enumValues)
                {
                    //Arrange
                    IdentificationResult identificationResult = new IdentificationResult(IdentificationResultType.InvalidReason, value.ToString());

                    //Act

                    //Assert
                    Assert.AreEqual(identificationResult.Error.ToString(), value.ToString());
                    Assert.IsNull(identificationResult.Data);
                }
            }
        }

    }
}
