using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class RecipientByNameAndAddressTests
    {
        [TestClass]
        public class ConstructorMethod : RecipientByNameAndAddressTests
        {
            private Comparator _comparator = new Comparator();

            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                const string fullName = "Ola Nordmann";
                const string addressLine1 = "Biskop Gunnerus Gate 14";
                const string postalCode = "0001";
                const string city = "Oslo";

                var recipientByNameAndAddress = new RecipientByNameAndAddress(fullName, addressLine1, postalCode, city);

                //Act

                //Assert
                Assert.AreEqual(fullName, recipientByNameAndAddress.FullName);
                Assert.AreEqual(postalCode, recipientByNameAndAddress.PostalCode);
                Assert.AreEqual(city, recipientByNameAndAddress.City);
                Assert.AreEqual(addressLine1, recipientByNameAndAddress.AddressLine1);
            }
        }
    }
}