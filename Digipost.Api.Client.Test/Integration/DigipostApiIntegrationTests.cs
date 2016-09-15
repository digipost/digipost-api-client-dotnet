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
using Digipost.Api.Client.Resources.Xml;
using Digipost.Api.Client.Test.Fakes;
using Moq;
using Xunit;

namespace Digipost.Api.Client.Test.Integration
{
    public class DigipostApiIntegrationTests
    {
        protected X509Certificate2 Certificate;
        protected ClientConfig ClientConfig;
        protected Uri Uri;

        public DigipostApiIntegrationTests()
        {
            ClientConfig = new ClientConfig("1337", Environment.Qa)
            {
                LogRequestAndResponse = false,
                TimeoutMilliseconds = 300000000
            };
            Uri = new Uri("/identification", UriKind.Relative);
            Certificate = TestProperties.Certificate();
        }

        internal AuthenticationHandler CreateHandlerChain(
            FakeResponseHandler fakehandler)
        {
            var loggingHandler = new LoggingHandler(fakehandler, ClientConfig);
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
                SendMessage(message, new FakeResponseHandler() { ResultCode = HttpStatusCode.OK, HttpContent = XmlResource.SendMessage.GetMessageDelivery()});
            }

            [Fact]
            public void ProperRequestSentRecipientByNameAndAddress()
            {
                var message = DomainUtility.GetSimpleMessageWithRecipientByNameAndAddress();

                SendMessage(message, new FakeResponseHandler() { ResultCode = HttpStatusCode.OK, HttpContent = XmlResource.SendMessage.GetMessageDelivery()});
            }

            [Fact]
            public void InternalServerErrorShouldCauseDigipostResponseException()
            {
                try
                {
                    var message = DomainUtility.GetSimpleMessageWithRecipientById();
                    const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                    var messageContent = new StringContent(string.Empty);

                    SendMessage(message, new FakeResponseHandler() {ResultCode = statusCode, HttpContent = messageContent});
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
                    var messageContent = XmlResource.SendMessage.GetError();

                    SendMessage(message, new FakeResponseHandler() { ResultCode = statusCode, HttpContent = messageContent });
                }
                catch (AggregateException e)
                {
                    var ex = e.InnerExceptions.ElementAt(0);

                    Assert.True(ex.GetType() == typeof (ClientResponseException));
                }
            }

            private void SendMessage(IMessage message, FakeResponseHandler fakeResponseHandler)
            {
                var fakehandler = fakeResponseHandler;
                var fakeHandlerChain = CreateHandlerChain(fakehandler);

                var mockFacktory = CreateMockFactoryReturningMessage(message, fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFacktory);

                digipostApi.SendMessage(message);
            }

            private Mock<DigipostActionFactory> CreateMockFactoryReturningMessage(IMessage message, AuthenticationHandler authenticationHandler)
            {
                var mockFacktory = new Mock<DigipostActionFactory>();
                mockFacktory.Setup(
                    f =>
                        f.CreateClass(message, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                            It.IsAny<Uri>()))
                    .Returns(new MessageAction(message, ClientConfig, Certificate, Uri)
                    {
                        HttpClient = new HttpClient(authenticationHandler) {BaseAddress = new Uri("http://tull")}
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
               Identify(identification);

            }

            [Fact]
            public void ProperRequestWithIdSent()
            {
                var identification = DomainUtility.GetPersonalIdentificationById();
                Identify(identification);
            }

            [Fact]
            public void ProperRequestWithNameAndAddressSent()
            {
                var identification = DomainUtility.GetPersonalIdentificationByNameAndAddress();
                Identify(identification);
            }

            private void Identify(IIdentification identification)
            {
                var fakehandler = new FakeResponseHandler() {ResultCode = HttpStatusCode.OK, HttpContent = XmlResource.Identification.GetResult()};
                var fakeHandlerChain = CreateHandlerChain(fakehandler);
                var mockFactory = CreateMockFactoryReturningIdentification(identification, fakeHandlerChain);

                var digipostApi = new DigipostApi(ClientConfig, Certificate);
                SetMockFactoryForDigipostApi(digipostApi, mockFactory);

                digipostApi.Identify(identification);
            }

            private Mock<DigipostActionFactory> CreateMockFactoryReturningIdentification(IIdentification identification, AuthenticationHandler authenticationHandler)
            {
                var mockFactory = new Mock<DigipostActionFactory>();
                mockFactory.Setup(
                    f =>
                        f.CreateClass(identification, It.IsAny<ClientConfig>(), It.IsAny<X509Certificate2>(),
                            It.IsAny<Uri>()))
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
            [Fact]
            public void ProperRequestSent()
            {
                var searchString = "jarand";

                var fakehandler = new FakeResponseHandler() { ResultCode = HttpStatusCode.OK, HttpContent = XmlResource.Search.GetResult()};
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
                            It.IsAny<Uri>()))
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