using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Tests;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Identify
{
    public class IdentificationTests
    {
        public class ConstructorMethod : IdentificationTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                var recipientByNameAndAddress = DomainUtility.GetRecipientByNameAndAddress();
                var identification = new Identification(recipientByNameAndAddress);

                //Act

                //Assert
                Assert.Equal(recipientByNameAndAddress, identification.DigipostRecipient);
            }
        }
    }
}