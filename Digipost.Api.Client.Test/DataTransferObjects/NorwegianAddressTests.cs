using Digipost.Api.Client.Common.Print;
using Xunit;

namespace Digipost.Api.Client.Tests.DataTransferObjects
{
    public class NorwegianAddressTests
    {
        public class ConstructorMethod : NorwegianAddressTests
        {
            [Fact]
            public void WhatYouAreTestingOnMethod()
            {
                //Arrange
                var norwegianAddress = new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");

                //Act

                //Assert
                Assert.Equal("0001", norwegianAddress.PostalCode);
                Assert.Equal("Oslo", norwegianAddress.City);
                Assert.Equal("Addr1", norwegianAddress.AddressLine1);
                Assert.Equal("Addr2", norwegianAddress.AddressLine2);
                Assert.Equal("Addr3", norwegianAddress.AddressLine3);
            }
        }
    }
}