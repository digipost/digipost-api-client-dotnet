using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identification;
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
            public void SetsIdentificationResultType()
            {
                //Arrange
                IdentificationResult identificationResult = new IdentificationResult(IdentificationResultType.Digipostaddress, "ola.nordmann#2433B");
                
                //Act

                //Assert
                Assert.AreEqual(identificationResult.IdentificationResultType, IdentificationResultType.Digipostaddress);
            }
        }

        [TestClass]
        public class SetResultByIdentificationResultTypeMethod : IdentificationTests
        {
            [TestMethod]
            public void ParsesAllInvalidResultsToEnum()
            {
                //Arrange
                IdentificationResult identificationResult = new IdentificationResult(IdentificationResultType.Invalidreason, "ola.nordmann#2433B");


                //Act

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public void ParsesAllErrorResultsToEnum()
            {
                //Arrange
                IdentificationResult identificationResult = new IdentificationResult(IdentificationResultType.Unidentifiedreason, "ola.nordmann#2433B");


                //Act

                //Assert
                Assert.Fail();
            }


            [TestMethod]
            public void ParsesAllOkMessagesToResult()
            {
                //Arrange

                //Act

                //Assert
                Assert.Fail();
                
            }

            [TestMethod]
            public void ThrowsExceptionIfNotIdentifiedAsOk()
            {
                //Arrange

                //Act

                //Assert
                Assert.Fail();
                
            }

            [TestMethod]
            public void ThrowsExceptionIfNotIdentifiedAsError()
            {
                //Arrange

                //Act

                //Assert
                Assert.Fail();

            }

        }

        [TestClass]
        public class ParseToIdentificationErrorMethod : IdentificationTests
        {
            [TestMethod]
            public void ParseToCorrectIdentificationError()
            {
                //Arrange

                //Act

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public void ThrowErrorOnNotEnumValue()
            {
                //Arrange

                //Act

                //Assert
                Assert.Fail();
                
            }
        }
    }
}
