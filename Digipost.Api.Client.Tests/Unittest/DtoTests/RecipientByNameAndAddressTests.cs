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
                RecipientByNameAndAddress recipientByNameAndAddress = new RecipientByNameAndAddress("Ola Nordmann", "Biskop Gunnerus Gate 14", "0001", "Oslo");

                //Act

                //Assert
                Assert.AreEqual("Ola Nordmann", recipientByNameAndAddress.FullName);
                Assert.AreEqual("0001", recipientByNameAndAddress.PostalCode);
                Assert.AreEqual("Oslo", recipientByNameAndAddress.City);
                Assert.AreEqual("Biskop Gunnerus Gate 14", recipientByNameAndAddress.AddressLine1);
            }
        }
    }
}
