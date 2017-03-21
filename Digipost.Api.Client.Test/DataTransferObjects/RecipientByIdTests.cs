using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Recipient;
using Xunit;

namespace Digipost.Api.Client.Tests.DataTransferObjects
{
    public class RecipientByIdTests
    {
        public class ConstructorMethod : RecipientByIdTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                const string testPerson = "ola.nordmann#2233";

                var recipientById = new RecipientById(
                    IdentificationType.DigipostAddress,
                    testPerson);

                //Act

                //Assert
                Assert.Equal(IdentificationType.DigipostAddress, recipientById.IdentificationType);
                Assert.Equal(testPerson, recipientById.Id);
            }
        }
    }
}