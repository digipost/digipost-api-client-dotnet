using Digipost.Api.Client.Domain.Print;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class NorwegianAddressTests
    {
        [TestClass]
        public class ConstructorMethod : NorwegianAddressTests
        {
            [TestMethod]
            public void WhatYouAreTestingOnMethod()
            {
                //Arrange
                var norwegianAddress = new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");

                //Act

                //Assert
                Assert.AreEqual("0001", norwegianAddress.PostalCode);
                Assert.AreEqual("Oslo", norwegianAddress.City);
                Assert.AreEqual("Addr1", norwegianAddress.AddressLine1);
                Assert.AreEqual("Addr2", norwegianAddress.AddressLine2);
                Assert.AreEqual("Addr3", norwegianAddress.AddressLine3);
            }
        }
    }
}