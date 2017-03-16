using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common.Actions;
using Xunit;

namespace Digipost.Api.Client.Test.Action
{
    public class DigipostActionFactoryTests
    {
        public class CreateClassMethod
        {
            [Fact]
            public void ReturnsProperIdentificationAction()
            {
                //Arrange
                var factory = new DigipostActionFactory();
                var identification = DomainUtility.GetPersonalIdentification();

                //Act
                var action = factory.CreateClass(identification, DomainUtility.GetClientConfig(), new X509Certificate2(), new Uri("/fakeuri", UriKind.Relative));

                //Assert
                Assert.Equal(typeof(IdentificationAction), action.GetType());
            }

            [Fact]
            public void ReturnsProperMessageAction()
            {
                //Arrange
                var factory = new DigipostActionFactory();
                var message = DomainUtility.GetSimpleMessageWithRecipientById();

                //Act
                var action = factory.CreateClass(message, DomainUtility.GetClientConfig(), new X509Certificate2(), new Uri("/fakeuri", UriKind.Relative));
                //Assert

                Assert.Equal(typeof(MessageAction), action.GetType());
            }
        }
    }
}