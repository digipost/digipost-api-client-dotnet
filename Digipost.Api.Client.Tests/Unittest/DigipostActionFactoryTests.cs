using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
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
            [TestMethod]
            [ExpectedException(typeof(ConfigException), "Wrongly initialized DigipostActionFactory with wrong type.")]
            public void InitializeUnknownActionThrowsException()
            {
                var factory = new DigipostActionFactory();
                factory.CreateClass(typeof(string), new ClientConfig(), new X509Certificate2(), "uri");
            }

            [TestMethod]
            public void ReturnsProperMessageAction()
            {
                var factory = new DigipostActionFactory();
                var action = factory.CreateClass(typeof(Message), new ClientConfig(), new X509Certificate2(), "uri");

                Assert.AreEqual(typeof(MessageAction), action.GetType());
            }

            [TestMethod]
            public void ReturnsProperIdentificationAction()
            {
                var factory = new DigipostActionFactory();
                var action = factory.CreateClass(typeof(Identification), new ClientConfig(), new X509Certificate2(), "uri");

                Assert.AreEqual(typeof(IdentificationAction), action.GetType());
            }    
        }
    }
}
