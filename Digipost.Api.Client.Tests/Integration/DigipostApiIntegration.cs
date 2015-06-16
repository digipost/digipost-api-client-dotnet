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
    public class DigipostApiIntegrationTests
    {
        protected ResourceUtility ResourceUtility;
        protected ClientConfig ClientConfig;
        protected string Uri;
        protected X509Certificate2 Certificate;

        [TestInitialize]
        public void Init()
        {
            ResourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");
            ClientConfig = new ClientConfig("1337");
            Uri = "identification";
            Certificate = TestProperties.Certificate();
        }

        [TestClass]
        public class SendMessageMethod : DigipostApiIntegrationTests
        {
            /// <summary>
            /// This integration test assures that the connection between handlers is correct and that a message is built and sent. 
            /// The ActionFactory is mocked to prevent actual HTTP-request to Digipost. 
            /// </summary>
            [TestMethod]
            public void ProperRequestSent()
            {
                var message = new Message(
                    new Recipient(IdentificationChoice.PersonalidentificationNumber, "00000000000"),
                    new Document("Integrasjonstjest", "txt", ResourceUtility.ReadAllBytes(true, "Vedlegg.txt"))
                    );

                try
                {
                    var fakehandler = new FakeMessageResponseHandler();
                    var loggingHandler = new LoggingHandler(fakehandler);
                    var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);


                    //Setup - init mock of ActionFactory to inject fake identification response handler
                    var mockFacktory = new Mock<DigipostActionFactory>();
                    mockFacktory.Setup(
                        f => f.CreateClass(message, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(), It.IsAny<string>()))
                        .Returns(new MessageAction(message, ClientConfig, Certificate, Uri)
                        {
                            ThreadSafeHttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) { DigipostActionFactory = mockFacktory.Object };

                    dpApi.SendMessage(message);
                }
                catch
                {
                    Assert.Fail();
                }
            }    
        }

        [TestClass]
        public class SendIdentifyMethod : DigipostApiIntegrationTests
        {
            /// <summary>
            /// This integration test assures that the connection between handlers is correct and that a message is built and sent. 
            /// The ActionFactory is mocked to prevent actual HTTP-request to Digipost. 
            /// </summary>
            [TestMethod]
            public void ProperRequestSent()
            {
                var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "00000000000");

                try
                {
                    var fakehandler = new FakeIdentificationResponseHandler();
                    var loggingHandler = new LoggingHandler(fakehandler);
                    var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);

                    //Setup - init mock of ActionFactory to inject fake identification response handler
                    var mockFacktory = new Mock<DigipostActionFactory>();
                    mockFacktory.Setup(
                        f => f.CreateClass(identification, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(), It.IsAny<string>()))
                        .Returns(new IdentificationAction(identification, ClientConfig, Certificate, Uri)
                        {
                            ThreadSafeHttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) { DigipostActionFactory = mockFacktory.Object };

                    var v = dpApi.Identify(identification);
                }
                catch
                {
                    Assert.Fail();
                }

            }
        }
       


    }
}
