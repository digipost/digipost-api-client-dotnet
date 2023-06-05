using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Print;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Print
{
    public class ForeignAddressTests
    {
        public class ConstructorMethod : ForeignAddressTests
        {
            [Fact]
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
                Assert.Equal(CountryIdentifier.Country, foreignAddress.CountryIdentifier);
                Assert.Equal("NO", foreignAddress.CountryIdentifierValue);
                Assert.Equal("Adresselinje1", foreignAddress.AddressLine1);
                Assert.Equal("Adresselinje2", foreignAddress.AddressLine2);
                Assert.Equal("Adresselinje3", foreignAddress.AddressLine3);
                Assert.Equal("Adresselinje4", foreignAddress.AddressLine4);
            }
        }
    }
}
