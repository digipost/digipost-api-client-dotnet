using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Handlers;
using Digipost.Api.Client.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Digipost.Api.Client.Tests.Integration
{
    [TestClass]
    public class DigipostApiTests
    {
        private static ResourceUtility _resourceUtility;
        private static ClientConfig _clientConfig;
        private static X509Certificate2 _certificate;
        private static string _uri;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _resourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");
            _clientConfig = new ClientConfig("1337");
            _certificate = new X509Certificate2(_resourceUtility.ReadAllBytes(true, "certificate.p12"), "abc123hest", X509KeyStorageFlags.Exportable);
            _uri = "identification";

        }

        [TestMethod]
        public void DigipostApi_SendMessageRequest_Integration()
        {
            var message = new Message(
                new Recipient(IdentificationChoice.PersonalidentificationNumber, "00000000000"),
                new Document("Integrasjonstjest", "txt", _resourceUtility.ReadAllBytes(true,"Vedlegg.txt"))
                );

            try
            {
                var fakehandler = new FakeMessageResponseHandler();
                var loggingHandler = new LoggingHandler(fakehandler);
                var authenticationHandler = new AuthenticationHandler(_clientConfig, _certificate, _uri, loggingHandler);


                //Setup - init mock of ActionFactory to inject fake identification response handler
                var mockFacktory = new Mock<DigipostActionFactory>();
                mockFacktory.Setup(
                    f => f.CreateClass(typeof(Message), It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(), It.IsAny<string>()))
                    .Returns(new MessageAction(_clientConfig, _certificate, _uri)
                    {
                        HttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                    });

                var dpApi = new DigipostApi(_clientConfig, _certificate) { DigipostActionFactory = mockFacktory.Object };

                dpApi.SendMessage(message);
            }
            catch
            {
                Assert.Fail();
            }
        }
        
        /// <summary>
        /// This integration test assures that the connection between handlers is correct and that a message is built and sent. 
        /// The ActionFactory is mocked to prevent actual HTTP-request to Digipost. 
        /// </summary>
        [TestMethod]
        public void DigipostApi_SendIdentifyRequest_Integration()
        {
            var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "00000000000");

            try
            {
                var fakehandler = new FakeIdentificationResponseHandler();
                var loggingHandler = new LoggingHandler(fakehandler);
                var authenticationHandler = new AuthenticationHandler(_clientConfig, _certificate, _uri, loggingHandler);
                

                //Setup - init mock of ActionFactory to inject fake identification response handler
                var mockFacktory = new Mock<DigipostActionFactory>();
                mockFacktory.Setup(
                    f => f.CreateClass(typeof(Identification),It.IsAny<ClientConfig>(),It.IsAny<X509Certificate2>(),It.IsAny<string>()))
                    .Returns(new IdentificationAction(_clientConfig, _certificate, _uri)
                    {
                        HttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                    });

                var dpApi = new DigipostApi(_clientConfig, _certificate) {DigipostActionFactory = mockFacktory.Object};

                dpApi.Identify(identification);
            }
            catch
            {
                Assert.Fail();
            }

        }


    }
}
