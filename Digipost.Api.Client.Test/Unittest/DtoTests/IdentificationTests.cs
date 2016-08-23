using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Test.Integration;
using Xunit;

namespace Digipost.Api.Client.Test.Unittest.DtoTests
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