using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class ForeignAddressTests
    {
        [TestClass]
        public class ConstructorMethod : ForeignAddressTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var foreignAddress = new ForeignAddress(
                    CountryIdentifier.Country,
                    "NO",
                    "Adresselinje1",
                    "Adresselinje2",
                    "Adresselinje3",
                    "Adresselinje4");

                //Act

                //Assert
                Assert.AreEqual(CountryIdentifier.Country, foreignAddress.CountryIdentifier);
                Assert.AreEqual("NO", foreignAddress.CountryIdentifierValue);
                Assert.AreEqual("Adresselinje1", foreignAddress.AddressLine1);
                Assert.AreEqual("Adresselinje2", foreignAddress.AddressLine2);
                Assert.AreEqual("Adresselinje3", foreignAddress.AddressLine3);
                Assert.AreEqual("Adresselinje4", foreignAddress.Addressline4);
            }
        }
    }
}