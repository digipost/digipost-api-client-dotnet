using System.IO;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest
{
    [TestClass]
    public class DigipostActionFactoryTests
    {
        [TestClass]
        public class CreateClassMethod
        {
            internal ResourceUtility ResourceUtility;

            [TestInitialize]
            public void TestInit()
            {
                ResourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");
            }

            [TestMethod]
            public void ReturnsProperMessageAction()
            {
                //Arrange
                var factory = new DigipostActionFactory();
                var message = new Message(
                        new Recipient(IdentificationChoice.PersonalidentificationNumber, "00000000000"),
                        new Document("Integrasjonstjest", "txt", ResourceUtility.ReadAllBytes(true, "Vedlegg.txt"))
                    );

                //Act
                var action = factory.CreateClass(message, new ClientConfig(), new X509Certificate2(), "uri");
                //Assert
                
                Assert.AreEqual(typeof(MessageAction), action.GetType());
            }

            [TestMethod]
            public void ReturnsProperIdentificationAction()
            {
                //Arrange
                var factory = new DigipostActionFactory();
                var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "00000000000");
                        
                //Act
                var action = factory.CreateClass(identification, new ClientConfig(), new X509Certificate2(), "uri");

                //Assert
                Assert.AreEqual(typeof(IdentificationAction), action.GetType());
            }
        }
    }
}
