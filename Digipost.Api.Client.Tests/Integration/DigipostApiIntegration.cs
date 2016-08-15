using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Handlers;
using Digipost.Api.Client.Tests.Fakes;
using Moq;
using Xunit;

namespace Digipost.Api.Client.Tests.Integration
{
    public class DigipostApiIntegrationTests
    {
        protected X509Certificate2 Certificate;
        protected ClientConfig ClientConfig;
        protected string Uri;

        public DigipostApiIntegrationTests()
        {
            ClientConfig = new ClientConfig("1337");
            Uri = "identification";
            Certificate = TestProperties.Certificate();
        }

        internal AuthenticationHandler CreateHandlerChain(
            FakeHttpClientHandlerResponse fakehandler)
        {
            var loggingHandler = new LoggingHandler(fakehandler);
            var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, Uri, loggingHandler);
            return authenticationHandler;
        }

        internal static void SetMockFactoryForDigipostApi(DigipostApi dpApi, Mock<DigipostActionFactory> mockFacktory)
        {
            dpApi.DigipostActionFactory = mockFacktory.Object;
        }

        public class SendMessageMethod : DigipostApiIntegrationTests
        {
            [Fact]
            public void ProperRequestSentRecipientById()
            {
                var message = DomainUtility.GetSimpleMessageWithRecipientById();

                var fakehandler = new FakeHttpClientHandlerForMessageResponse();
                var fakeHandlerChain = CreateHandlerChain(fakehandler);

                var mockFacktory = CreateMockFactoryReturningMessage(message, fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFacktory);

                digipostApi.SendMessage(message);
            }

            [Fact]
            public void ProperRequestSentRecipientByNameAndAddress()
            {
                var message = DomainUtility.GetSimpleMessageWithRecipientByNameAndAddress();

                var fakehandler = new FakeHttpClientHandlerForMessageResponse();
                var fakeHandlerChain = CreateHandlerChain(fakehandler);

                var mockFacktory = CreateMockFactoryReturningMessage(message, fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFacktory);

                digipostApi.SendMessage(message);
            }

            [Fact]
            public void InternalServerErrorShouldCauseDigipostResponseException()
            {
                try
                {
                    var message = DomainUtility.GetSimpleMessageWithRecipientById();

                    const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                    var messageContent = new StringContent(string.Empty);

                    var fakehandler = CreateFakeMessageHttpResponse(statusCode, messageContent);
                    var fakeHandlerChain = CreateHandlerChain(fakehandler);
                    var mockFacktory = CreateMockFactoryReturningMessage(message, fakeHandlerChain);

                    var digipostApi = new DigipostApi(ClientConfig, Certificate);
                    SetMockFactoryForDigipostApi(digipostApi, mockFacktory);

                    digipostApi.SendMessage(message);
                }
                catch (AggregateException e)
                {
                    var ex = e.InnerExceptions.ElementAt(0);
                    Assert.True(ex.GetType() == typeof (ClientResponseException));
                }
            }

            [Fact]
            public void ShouldSerializeErrorMessage()
            {
                try
                {
                    var message = DomainUtility.GetSimpleMessageWithRecipientById();

                    const HttpStatusCode statusCode = HttpStatusCode.NotFound;
                    var messageContent = new StringContent(
                        @"<?xml version=""1.0"" encoding=""UTF - 8"" standalone=""yes""?><error xmlns=""http://api.digipost.no/schema/v6""><error-code>UNKNOWN_RECIPIENT</error-code><error-message>The recipient does not have a Digipost account.</error-message><error-type>CLIENT_DATA</error-type></error>");

                    var fakehandler = CreateFakeMessageHttpResponse(statusCode, messageContent);
                    var fakeHandlerChain = CreateHandlerChain(fakehandler);
                    var mockFacktory = CreateMockFactoryReturningMessage(message, fakeHandlerChain);

                    var digipostApi = new DigipostApi(ClientConfig, Certificate);
                    SetMockFactoryForDigipostApi(digipostApi, mockFacktory);

                    digipostApi.SendMessage(message);
                }
                catch (AggregateException e)
                {
                    var ex = e.InnerExceptions.ElementAt(0);

                    Assert.True(ex.GetType() == typeof (ClientResponseException));
                }
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

            private Mock<DigipostActionFactory> CreateMockFactoryReturningMessage(IMessage message, AuthenticationHandler authenticationHandler)
            {
                var mockFacktory = new Mock<DigipostActionFactory>();
                mockFacktory.Setup(
                    f =>
                        f.CreateClass(message, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                            It.IsAny<string>()))
                    .Returns(new MessageAction(message, ClientConfig, Certificate, Uri)
                    {
                        HttpClient =
                            new HttpClient(authenticationHandler) {BaseAddress = new Uri("http://tull")}
                    });
                return mockFacktory;
            }
        }

        public class SendIdentifyMethod : DigipostApiIntegrationTests
        {
            [Fact]
            public void ProperRequestSent()
            {
                var identification = DomainUtility.GetPersonalIdentification();

                var fakehandler = new FakeHttpClientHandlerForIdentificationResponse();
                var fakeHandlerChain = CreateHandlerChain(fakehandler);

                var mockFactory = CreateMockFactoryReturningIdentification(identification, fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFactory);

                digipostApi.Identify(identification);
            }

            [Fact]
            public void ProperRequestWithIdSent()
            {
                var identification = DomainUtility.GetPersonalIdentificationById();

                var fakehandler = new FakeHttpClientHandlerForIdentificationResponse();
                var fakeHandlerChain = CreateHandlerChain(fakehandler);
                var mockFactory = CreateMockFactoryReturningIdentification(identification, fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFactory);

                digipostApi.Identify(identification);
            }

            [Fact]
            public void ProperRequestWithNameAndAddressSent()
            {
                var identification = DomainUtility.GetPersonalIdentificationByNameAndAddress();

                var fakehandler = new FakeHttpClientHandlerForIdentificationResponse();
                var fakeHandlerChain = CreateHandlerChain(fakehandler);
                var mockFactory = CreateMockFactoryReturningIdentification(identification, fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFactory);

                digipostApi.Identify(identification);
            }

            private Mock<DigipostActionFactory> CreateMockFactoryReturningIdentification(IIdentification identification,
                AuthenticationHandler authenticationHandler)
            {
                var mockFactory = new Mock<DigipostActionFactory>();
                mockFactory.Setup(
                    f =>
                        f.CreateClass(identification, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                            It.IsAny<string>()))
                    .Returns(new IdentificationAction(identification, ClientConfig, Certificate, Uri)
                    {
                        HttpClient =
                            new HttpClient(authenticationHandler) {BaseAddress = new Uri("http://tull")}
                    });
                return mockFactory;
            }
        }

        public class SearchMethod : DigipostApiIntegrationTests
        {
            /// <summary>
            ///     This integration test assures that the connection between handlers is correct and that a message is built and sent.
            ///     The ActionFactory is mocked to prevent actual HTTP-request to Digipost.
            /// </summary>
            [Fact]
            public void ProperRequestSent()
            {
                var searchString = "marit";

                var fakehandler = new FakeHttpClientHandlerForSearchResponse();
                var fakeHandlerChain = CreateHandlerChain(fakehandler);

                var mockFacktory = CreateMockFactoryReturningSearch(fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFacktory);

                var result = digipostApi.Search(searchString);

                Assert.NotNull(result);
            }

            private Mock<DigipostActionFactory> CreateMockFactoryReturningSearch(AuthenticationHandler fakeHandlerChain)
            {
                var mockFacktory = new Mock<DigipostActionFactory>();
                mockFacktory.Setup(
                    f =>
                        f.CreateClass(It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                            It.IsAny<string>()))
                    .Returns(new GetByUriAction(null, ClientConfig, Certificate, Uri)
                    {
                        HttpClient =
                            new HttpClient(fakeHandlerChain) {BaseAddress = new Uri("http://tull")}
                    });
                return mockFacktory;
            }
        }
    }
}