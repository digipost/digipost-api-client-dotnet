using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using Xunit;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
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