using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.DataTransferObject;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.Tests.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class DtoConverterTests
    {
        readonly Comparator _comparator = new Comparator();

        [TestClass]
        public class ToDataTransferObjectMethod : DtoConverterTests
        {
            [TestMethod]
            public void Identification()
            {
                //Arrange
                Identification source = new Identification(IdentificationChoice.DigipostAddress, "Ola.Nordmann#244BB2");
                IdentificationDataTransferObject expectedDto = new IdentificationDataTransferObject(IdentificationChoice.DigipostAddress, "Ola.Nordmann#244BB2");

                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }

        [TestClass]
        public class FromDataTransferObjectMethod : DtoConverterTests
        {
            [TestMethod]
            public void IdentificationResultFromPersonalIdentificationNumber()
            {
                //Arrange
                IdentificationResultDataTransferObject source = new IdentificationResultDataTransferObject();
                source.IdentificationResultCode = IdentificationResultCode.Digipost;
                source.IdentificationValue = null;
                source.IdentificationResultType = IdentificationResultType.DigipostAddress;

                IdentificationResult expected = new IdentificationResult(IdentificationResultType.DigipostAddress, "");

                //Act
                var actual = DtoConverter.FromDataTransferObject(source);

                //Assert
                Assert.AreEqual(source.IdentificationResultType, expected.ResultType);
                Assert.AreEqual("", actual.Data);
                Assert.AreEqual(null, actual.Error);
            }

            [TestMethod]
            public void IdentificationResultFromPersonByNameAndAddress()
            {
                //Arrange
                IdentificationResultDataTransferObject source = new IdentificationResultDataTransferObject();
                source.IdentificationResultCode = IdentificationResultCode.Digipost;
                source.IdentificationValue = "jarand.bjarte.t.k.grindheim#8DVE";
                source.IdentificationResultType = IdentificationResultType.DigipostAddress;

                IdentificationResult expected = new IdentificationResult(IdentificationResultType.DigipostAddress, "jarand.bjarte.t.k.grindheim#8DVE");

                //Act
                IdentificationResult actual = DtoConverter.FromDataTransferObject(source);

                //Assert
                Assert.AreEqual(source.IdentificationValue, actual.Data);
                Assert.AreEqual(source.IdentificationResultType, actual.ResultType);
                Assert.AreEqual(null, actual.Error);
            }

            [TestMethod]
            public void IdentificationResultFromUnknownDigipostAddress()
            {
                //Arrange
                IdentificationResultDataTransferObject source = new IdentificationResultDataTransferObject();
                source.IdentificationResultCode = IdentificationResultCode.Unidentified;
                source.IdentificationValue = "NotFound";
                source.IdentificationResultType = IdentificationResultType.UnidentifiedReason;

                IdentificationResult expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, "NotFound");

                //Act
                var actual = DtoConverter.FromDataTransferObject(source);

                //Assert
                Assert.AreEqual(source.IdentificationResultType, actual.ResultType);
                Assert.AreEqual(null, actual.Data);
                Assert.AreEqual(source.IdentificationValue.ToString(),actual.Error.ToString());
            }


        }

    }
}
