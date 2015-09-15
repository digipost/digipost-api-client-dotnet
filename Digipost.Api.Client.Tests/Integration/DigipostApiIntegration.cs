using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.SendMessage;
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
                            ThreadSafeHttpClient =
                                new HttpClient(authenticationHandler) {BaseAddress = new Uri("http://tull")}
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) {DigipostActionFactory = mockFacktory.Object};

                    dpApi.SendMessage(message);

                    Assert.AreEqual(1, fakehandler.CalledCount, "The httpClient has been called more than expected.");
                }
                catch (Exception exception)
                {
                    Assert.Fail(exception.Message);
                }
            }

            /// <summary>
            ///     This integration test assures that the connection between handlers is correct and that a message is built and sent.
            ///     The ActionFactory is mocked to prevent actual HTTP-request to Digipost.
            /// </summary>
            [TestMethod]
            public void InternalServerErrorShouldCauseDigipostResponseException()
            {
                try
                {
                    var message = DomainUtility.GetSimpleMessage();

                    const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                    var messageContent = new StringContent(string.Empty);

                    var fakehandler = CreateFakeMessageHttpResponse(statusCode, messageContent);
                    var fakeHandlerChain = CreateHandlerChain(fakehandler);
                    var mockFacktory = CreateMockFactoryReturningMessage(message, fakeHandlerChain);

                    var dpApi = new DigipostApi(ClientConfig, Certificate);
                    SetMockFactoryForDigipostApi(dpApi, mockFacktory);

                    dpApi.SendMessage(message);

                    Assert.AreEqual(1, fakehandler.CalledCount, "The httpClient has been called more than expected.");
                }
                catch (AggregateException e)
                {
                    var ex = e.InnerExceptions.ElementAt(0);
                    Assert.IsTrue(ex.GetType() == typeof(ClientResponseException));
                }
            }
            
            [TestMethod]
            public void ShouldSerializeErrorMessage()
            {
                try
                {
                    var message = DomainUtility.GetSimpleMessage();

                    const HttpStatusCode statusCode = HttpStatusCode.NotFound;
                    var messageContent = new StringContent(
                        @"<?xml version=""1.0"" encoding=""UTF - 8"" standalone=""yes""?><error xmlns=""http://api.digipost.no/schema/v6""><error-code>UNKNOWN_RECIPIENT</error-code><error-message>The recipient does not have a Digipost account.</error-message><error-type>CLIENT_DATA</error-type></error>");
                    
                    var fakehandler = CreateFakeMessageHttpResponse(statusCode, messageContent);
                    var fakeHandlerChain = CreateHandlerChain(fakehandler);
                    var mockFacktory = CreateMockFactoryReturningMessage(message, fakeHandlerChain);

                    var dpApi = new DigipostApi(ClientConfig, Certificate);
                    SetMockFactoryForDigipostApi(dpApi, mockFacktory);

                    dpApi.SendMessage(message);

                    Assert.AreEqual(1, fakehandler.CalledCount, "The httpClient has been called more than expected.");
                }
                catch (AggregateException e)
                {
                    var ex = e.InnerExceptions.ElementAt(0);
                    Assert.IsTrue(ex.GetType() == typeof(ClientResponseException));
                }
            }

            private static void SetMockFactoryForDigipostApi(DigipostApi dpApi, Mock<DigipostActionFactory> mockFacktory)
            {
                dpApi.DigipostActionFactory = mockFacktory.Object;
            }

            private static FakeHttpClientHandlerForMessageResponse CreateFakeMessageHttpResponse(HttpStatusCode statusCode,
                StringContent messageContent)
            {
                var fakehandler = new FakeHttpClientHandlerForMessageResponse
                {
                    ResultCode = statusCode,
                    HttpContent = messageContent
                };
                return fakehandler;
            }

            private AuthenticationHandler CreateHandlerChain(
                FakeHttpClientHandlerForMessageResponse fakehandler)
            {
                var loggingHandler = new LoggingHandler(fakehandler);
                var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);
                return authenticationHandler;
            }

            private Mock<DigipostActionFactory> CreateMockFactoryReturningMessage(IMessage message, AuthenticationHandler authenticationHandler)
            {
                var mockFacktory = new Mock<DigipostActionFactory>();
                mockFacktory.Setup(
                    f =>
                        f.CreateClass(message, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                            It.IsAny<string>()))
                    .Returns(new MessageAction(message, ClientConfig, Certificate, Uri)
                    {
                        ThreadSafeHttpClient =
                            new HttpClient(authenticationHandler) { BaseAddress = new Uri("http://tull") }
                    });
                return mockFacktory;
            }
        }

        [TestClass]
        public class SendIdentifyMethod : DigipostApiIntegrationTests
        {
            /// <summary>
            ///     This integration test assures that the connection between handlers is correct and that a message is built and sent.
            ///     The ActionFactory is mocked to prevent actual HTTP-request to Digipost.
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
                            ThreadSafeHttpClient =
                                new HttpClient(authenticationHandler) {BaseAddress = new Uri("http://tull")}
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) {DigipostActionFactory = mockFacktory.Object};

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
                            ThreadSafeHttpClient =
                                new HttpClient(authenticationHandler) {BaseAddress = new Uri("http://tull")}
                        });

                    var dpApi = new DigipostApi(ClientConfig, Certificate) {DigipostActionFactory = mockFacktory.Object};

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