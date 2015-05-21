using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Smoke
{
    [TestClass]
    public class DigipostApiTests
    {
        private static ResourceUtility _resourceUtility;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _resourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");
        }

        [TestMethod]
        public void SendMessageTestSuccess()
        {
            try
            {
                var api = new DigipostApi(new ClientConfig("13371337"), new X509Certificate2())
                {
                    DigipostActionFactory = new FakeDigipostActionFactory()
                };
            
                var message = new Message(
                    new Recipient(IdentificationChoice.PersonalidentificationNumber, "01013300001"),
                    new Document("DigipostApiTest", "txt", _resourceUtility.ReadAllBytes(true, "Hoveddokument.txt")));

                api.SendMessage(message);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SendIdentificationTestSuccess()
        {
            try
            {
                var api = new DigipostApi(new ClientConfig("13371337"), new X509Certificate2())
                {
                    DigipostActionFactory = new FakeDigipostActionFactory()
                };

                var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "01013300001");

                api.Identify(identification);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

        }


    }
}
