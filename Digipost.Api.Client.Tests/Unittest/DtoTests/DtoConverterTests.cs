using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identification;
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
                IdentificationDto expectedDto = new IdentificationDto(IdentificationChoice.DigipostAddress, "Ola.Nordmann#244BB2");

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
            public void IdentificationResultFromPersonalIdentificationNumberIdentification()
            {
                //Arrange
                IdentificationResultDto source = new IdentificationResultDto();
                source.IdentificationResultCode = IdentificationResultCode.Digipost;
                source.IdentificationValue = null;
                source.IdentificationResultType = IdentificationResultType.DigipostAddress;
                
                IdentificationResult expected = new IdentificationResult(IdentificationResultType.DigipostAddress, "");
                
                //Act
                var actual = DtoConverter.FromDataTransferObject(source);

                //Assert
                _comparator.AreEqual(expected, actual);
            }
        }

    }
}
