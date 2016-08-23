using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Test.Integration;
using Xunit;

namespace Digipost.Api.Client.Test.Unittest
{
    public class DigipostActionFactoryTests
    {
        public class CreateClassMethod
        {
            internal ResourceUtility ResourceUtility;

            public CreateClassMethod()
            {
                ResourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");
            }

            [Fact]
            public void ReturnsProperMessageAction()
            {
                //Arrange
                var factory = new DigipostActionFactory();
                var message = DomainUtility.GetSimpleMessageWithRecipientById();

                //Act
                var action = factory.CreateClass(message, DomainUtility.GetClientConfig(), new X509Certificate2(), "uri");
                //Assert

                Assert.Equal(typeof (MessageAction), action.GetType());
            }

            [Fact]
            public void ReturnsProperIdentificationAction()
            {
                //Arrange
                var factory = new DigipostActionFactory();
                var identification = DomainUtility.GetPersonalIdentification();

                //Act
                var action = factory.CreateClass(identification, DomainUtility.GetClientConfig(), new X509Certificate2(), "uri");

                //Assert
                Assert.Equal(typeof (IdentificationAction), action.GetType());
            }
        }
    }
}