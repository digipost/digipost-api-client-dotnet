using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Utilities;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Utilities
{
    public class UuidInteropTests
    {
        [Fact]
        public void InteropWithKnownJavaExample()
        {
            //Arrange
            var startString = "per er kul";

            //Act
            var result = UUIDInterop.NameUUIDFromBytes(startString);

            //Assert
            Assert.Equal("46f45680-1c6e-3425-bc1b-182a9928549f", result);
        }
    }
}
