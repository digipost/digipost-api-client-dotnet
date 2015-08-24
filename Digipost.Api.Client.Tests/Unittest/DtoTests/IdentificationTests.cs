using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Utilities;
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
                Identification identification = new Identification(IdentificationChoiceType.DigipostAddress, "Ola.nordmann#234HH");
                
                //Act
                
                //Assert
                Assert.AreEqual(IdentificationChoiceType.DigipostAddress, identification.IdentificationChoiceType);
                Assert.AreEqual("Ola.nordmann#234HH", (string) identification.Data);
            }

            [TestMethod]
            public void InitializeProperlyForRecipientInitialization()
            {
                //Arrange
                var recipientByNameAndAddress = new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Gateveien 2");
                Identification identification = new Identification(recipientByNameAndAddress);

                //Act

                //Assert

                IEnumerable<IDifference> differences;
                Comparator.AreEqual(recipientByNameAndAddress, (RecipientByNameAndAddress) identification.Data, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }
    }
}
