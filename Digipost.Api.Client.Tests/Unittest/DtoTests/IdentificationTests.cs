using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identification;
using Digipost.Api.Client.Tests.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class IdentificationTests
    {
        public IComparator Comparator = new Comparator();

        [TestClass]
        public class ConstructorMethod : IdentificationTests
        {
            [TestMethod]
            public void InitializeProperlyForStringInitialization()
            {
                //Arrange
                Identification identification = new Identification(IdentificationChoice.DigipostAddress, "Ola.nordmann#234HH");
                IdentificationDto identificationDto = new IdentificationDto(IdentificationChoice.DigipostAddress, "Ola.nordmann#234HH");
                
                //Act

                //Assert
                IEnumerable<IDifference> differences;
                Comparator.AreEqual(identificationDto, identification.DataTransferObject, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void InitializeProperlyForRecipientInitialization()
            {
                //Arrange
                Identification identification = new Identification(new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Gateveien 2"));
                IdentificationDto identificationDto = new IdentificationDto(new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Gateveien 2"));

                //Act

                //Assert
                IEnumerable<IDifference> differences;
                Comparator.AreEqual(identificationDto, identification.DataTransferObject, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }

        [TestClass]
        public class IdentificationValueField : IdentificationTests
        
        {
            [TestMethod]
            public void ReturnsCorrectIdentificationValueForStringInitialization()
            {
                //Arrange
                Identification identification = new Identification(IdentificationChoice.DigipostAddress, "Ola.nordmann#234HH");

                //Act

                //Assert
                Assert.AreEqual("Ola.nordmann#234HH", (string)identification.IdentificationValue);
            }

            [TestMethod]
            public void ReturnsCorrectIdentificationValueForRecipientInitialization()
            {
                //Arrange
                var recipient = new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Gateveien 2");
                Identification identification = new Identification(new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Gateveien 2"));

                //Act

                //Assert
                IEnumerable<IDifference> differences;
                Comparator.AreEqual(recipient, identification.IdentificationValue, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }

        [TestClass]
        public class IdentificationTypeField : IdentificationTests
        {
            [TestMethod]
            public void ReturnsCorrectIdentificationType() 
            {
                //Arrange
                Identification identification = new Identification(IdentificationChoice.DigipostAddress, "Ola.nordmann#234HH");

                //Act

                //Assert
                Assert.AreEqual(IdentificationChoice.DigipostAddress, identification.IdentificationType);
            }
        }

    }
}
