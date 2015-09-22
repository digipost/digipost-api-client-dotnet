using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class IdentificationByNameAndAddressTests
    {
        [TestClass]
        public class ConstructorMethod : IdentificationByNameAndAddressTests
        {
            Comparator _comparator = new Comparator();

            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var recipientByNameAndAddress = DomainUtility.GetRecipientByNameAndAddress();
                IdentificationByNameAndAddress identificationByNameAndAddress = new IdentificationByNameAndAddress(
                    recipientByNameAndAddress);

                //Act

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(recipientByNameAndAddress, identificationByNameAndAddress.RecipientByNameAndAddressDataTranferObject,
                    out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }

        [TestClass]
        public class DataMethod : IdentificationByNameAndAddressTests
        {
            [TestMethod]
            public void ReturnsCorrectData()
            {
                //Arrange
                IdentificationByNameAndAddress identificationByNameAndAddress =
                    new IdentificationByNameAndAddress(DomainUtility.GetRecipientByNameAndAddress());

                //Act

                //Assert
                Assert.AreSame(identificationByNameAndAddress.Data, identificationByNameAndAddress.RecipientByNameAndAddressDataTranferObject);
            } 
        }

        [TestClass]
        public class IdentificationChoiceTypeMethod : IdentificationByNameAndAddressTests
        {
            [TestMethod]
            public void ReturnsNameAndAddress()
            {
                //Arrange
                IdentificationByNameAndAddress identificationByNameAndAddress =
                   new IdentificationByNameAndAddress(DomainUtility.GetRecipientByNameAndAddress());

                //Act

                //Assert
                Assert.AreEqual(IdentificationChoiceType.NameAndAddress, identificationByNameAndAddress.IdentificationChoiceType);
            }
        }

        
    }
}
