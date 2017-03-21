using Digipost.Api.Client.Common.Identify;
using Xunit;

namespace Digipost.Api.Client.Tests.DataTransferObjects
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