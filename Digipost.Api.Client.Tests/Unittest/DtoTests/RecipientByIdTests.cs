using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class RecipientByIdTests
    {
        [TestClass]
        public class ConstructorMethod : RecipientByIdTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                const string testPerson = "ola.nordmann#2233";

                RecipientById recipientById = new RecipientById(
                    IdentificationType.DigipostAddress, 
                    testPerson);

                //Act

                //Assert
                Assert.AreEqual(IdentificationType.DigipostAddress, recipientById.IdentificationType);
                Assert.AreEqual(testPerson, recipientById.Id);
            } 
        }
    }
}
