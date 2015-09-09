using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Handlers;
using Digipost.Api.Client.Tests.Fakes;
using Digipost.Api.Client.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Digipost.Api.Client.Tests.Integration
{
    [TestClass]
    public class DigipostApiIntegrationTests
    {
        protected X509Certificate2 Certificate;
        protected ClientConfig ClientConfig;

        protected string Uri;


        [TestInitialize]
        public void Init()
        {

            ClientConfig = new ClientConfig("1337");
            Uri = "identification";
            Certificate = TestProperties.Certificate();
        }

        [TestClass]
        public class SendMessageMethod : DigipostApiIntegrationTests
        {


            /// <summary>
            ///     This integration test assures that the connection between handlers is correct and that a message is built and sent.
            ///     The ActionFactory is mocked to prevent actual HTTP-request to Digipost.
            /// </summary>
            [TestMethod]
            public void ProperRequestSent()
            {
                var message = DomainUtility.GetSimpleMessage();

                try
                {
                    var fakehandler = new FakeHttpClientHandlerForMessageResponse();
                    var loggingHandler = new LoggingHandler(fakehandler);
                    var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);


                    //Setup - init mock of ActionFactory to inject fake identification response handler
                    var mockFacktory = new Mock<DigipostActionFactory>();
                    mockFacktory.Setup(
                        f =>
                            f.CreateClass(message, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                                It.IsAny<string>()))
                        .Returns(new MessageAction(message, ClientConfig, Certificate, Uri)
                        {
                            ThreadSafeHttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) { DigipostActionFactory = mockFacktory.Object };

                    dpApi.SendMessage(message);

                    Assert.AreEqual(1, fakehandler.CalledCount, "The httpClient has been called more than expected.");
                }
                catch (Exception exception)
                {
                    Assert.Fail(exception.Message);
                }
            }

            /// <summary>
            /// This integration test assures that the connection between handlers is correct and that a message is built and sent.
            /// The ActionFactory is mocked to prevent actual HTTP-request to Digipost.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ClientResponseException))]
            public void InternalServerErrorShouldCauseDigipostResponseException()
            {
                var message = DomainUtility.GetSimpleMessage();

                var fakehandler = new FakeHttpClientHandlerForMessageResponse
                {
                    ResultCode = HttpStatusCode.InternalServerError,
                    HttpContent = new StringContent(string.Empty)
                };
                var loggingHandler = new LoggingHandler(fakehandler);
                var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);


                //Setup - init mock of ActionFactory to inject fake identification response handler
                var mockFacktory = new Mock<DigipostActionFactory>();
                mockFacktory.Setup(
                    f =>
                        f.CreateClass(message, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                            It.IsAny<string>()))
                    .Returns(new MessageAction(message, ClientConfig, Certificate, Uri)
                    {
                        ThreadSafeHttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                    });

                var dpApi = new DigipostApi(ClientConfig, Certificate) { DigipostActionFactory = mockFacktory.Object };

                var result = dpApi.SendMessage(message);

                Assert.AreEqual(1, fakehandler.CalledCount, "The httpClient has been called more than expected.");
            }

            [TestMethod]
            public void ShouldSerializeErrorMessage()
            {
                var message = DomainUtility.GetSimpleMessage();

                try
                {
                    var fakehandler = new FakeHttpClientHandlerForMessageResponse
                    {
                        ResultCode = HttpStatusCode.NotFound,
                        HttpContent =
                            new StringContent(
                                @"<?xml version=""1.0"" encoding=""UTF - 8"" standalone=""yes""?><error xmlns=""http://api.digipost.no/schema/v6""><error-code>UNKNOWN_RECIPIENT</error-code><error-message>The recipient does not have a Digipost account.</error-message><error-type>CLIENT_DATA</error-type></error>")
                    };

                    var loggingHandler = new LoggingHandler(fakehandler);
                    var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);


                    //Setup - init mock of ActionFactory to inject fake identification response handler
                    var mockFacktory = new Mock<DigipostActionFactory>();
                    mockFacktory.Setup(
                        f =>
                            f.CreateClass(message, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                                It.IsAny<string>()))
                        .Returns(new MessageAction(message, ClientConfig, Certificate, Uri)
                        {
                            ThreadSafeHttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) { DigipostActionFactory = mockFacktory.Object };

                    dpApi.SendMessage(message);

                    Assert.AreEqual(1, fakehandler.CalledCount, "The httpClient has been called more than expected.");
                }
                catch (ClientResponseException exception)
                {
                    Assert.IsNotNull(exception.Error);
                    
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
                var identification = DomainUtility.GetPersonalIdentification();

                try
                {
                    var fakehandler = new FakeHttpClientHandlerForIdentificationResponse();
                    var loggingHandler = new LoggingHandler(fakehandler);
                    var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);


                    //Setup - init mock of ActionFactory to inject fake identification response handler
                    var mockFacktory = new Mock<DigipostActionFactory>();
                    mockFacktory.Setup(
                        f =>
                            f.CreateClass(identification, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                                It.IsAny<string>()))
                        .Returns(new IdentificationAction(identification, ClientConfig, Certificate, Uri)
                        {
                            ThreadSafeHttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) { DigipostActionFactory = mockFacktory.Object };

                    dpApi.Identify(identification);
                }
                catch (Exception exception)
                {
                    Assert.Fail(exception.Message);
                }
            }
        }

        [TestClass]
        public class SearchMethod : DigipostApiIntegrationTests
        {
            /// <summary>
            ///     This integration test assures that the connection between handlers is correct and that a message is built and sent.
            ///     The ActionFactory is mocked to prevent actual HTTP-request to Digipost.
            /// </summary>
            [TestMethod]
            public void ProperRequestSent()
            {
                var searchString = "marit";

                try
                {
                    var fakehandler = new FakeHttpClientHandlerForSearchResponse();
                    var loggingHandler = new LoggingHandler(fakehandler);
                    var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);


                    //Setup - init mock of ActionFactory to inject fake identification response handler
                    var mockFacktory = new Mock<DigipostActionFactory>();
                    mockFacktory.Setup(
                        f =>
                            f.CreateClass(It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                                It.IsAny<string>()))
                        .Returns(new GetByUriAction(null, ClientConfig, Certificate, Uri)
                        {
                            ThreadSafeHttpClient = new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) { DigipostActionFactory = mockFacktory.Object };

                    var result = dpApi.Search(searchString);
                    Assert.IsNotNull(result);

                 
                }
                catch (Exception exception)
                {
                    Assert.Fail(exception.Message);
                }
            }
        }
    }
}
