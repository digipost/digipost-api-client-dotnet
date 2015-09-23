using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class IdentificationTests
    {
        [TestClass]
        public class ConstructorMethod : IdentificationTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var recipientByNameAndAddress = DomainUtility.GetRecipientByNameAndAddress();
                Identification identification = new Identification(recipientByNameAndAddress);

                //Act

                //Assert
                Assert.AreEqual(recipientByNameAndAddress, identification.DigipostRecipient);
            }
        }

    }
}
