using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class RecipientByNameAndAddressTests
    {
        [TestClass]
        public class ConstructorMethod : RecipientByNameAndAddressTests
        {
            Comparator _comparator = new Comparator();

            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                RecipientByNameAndAddress recipientByNameAndAddress = new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Biskop Gunnerus Gate 14");

                //Act

                //Assert
                Assert.AreEqual("Ola Nordmann", recipientByNameAndAddress.FullName);
                Assert.AreEqual("0001", recipientByNameAndAddress.PostalCode);
                Assert.AreEqual("Oslo", recipientByNameAndAddress.City);
                Assert.AreEqual("Biskop Gunnerus Gate 14", recipientByNameAndAddress.AddressLine1);
            }

            [TestMethod]
            public void ConstructorWithPrintDetails()
            {
                //Arrange
                IPrintDetails printDetails = DomainUtility.GetPrintDetails();
                RecipientByNameAndAddress recipientByNameAndAddress = new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Biskop Gunnerus Gate 14", printDetails);

                //Act

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(recipientByNameAndAddress.PrintDetails, printDetails, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }
    }
}
