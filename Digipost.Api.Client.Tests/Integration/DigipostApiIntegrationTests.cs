using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Exceptions;
using Digipost.Api.Client.Common.Handlers;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Resources.Xml;
using Digipost.Api.Client.Send;
using Digipost.Api.Client.Tests.Fakes;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Integration
{
    public class DigipostApiIntegrationTests
    {
        protected X509Certificate2 Certificate;
        protected ClientConfig ClientConfig;
        protected Uri Uri;

        public DigipostApiIntegrationTests()
        {
            ClientConfig = new ClientConfig(new Broker(1337), Environment.Production)
            {
                LogRequestAndResponse = false,
                TimeoutMilliseconds = 300000000
            };
            Uri = new Uri("/identification", UriKind.Relative);
            Certificate = CertificateResource.Certificate();
        }

        internal AuthenticationHandler CreateHandlerChain(
            FakeResponseHandler fakehandler)
        {
            var loggingHandler = new LoggingHandler(fakehandler, ClientConfig);
            var authenticationHandler = new AuthenticationHandler(ClientConfig, Certificate, loggingHandler);
            return authenticationHandler;
        }

        private SendMessageApi GetDigipostApi(FakeResponseHandler fakeResponseHandler)
        {
            var fakeHandlerChain = CreateHandlerChain(fakeResponseHandler);
            var httpClient = new HttpClient(fakeHandlerChain) {BaseAddress = new Uri("http://www.fakeBaseAddress.no")};
            var requestHelper = new RequestHelper(httpClient) {HttpClient = httpClient};

            var digipostApi = new SendMessageApi(new SendRequestHelper(requestHelper));
            return digipostApi;
        }

        public class SendMessageMethod : DigipostApiIntegrationTests
        {
            private void SendMessage(IMessage message, FakeResponseHandler fakeResponseHandler)
            {
                var digipostApi = GetDigipostApi(fakeResponseHandler);

                digipostApi.SendMessage(message);
            }

            [Fact]
            public void InternalServerErrorShouldCauseDigipostResponseException()
            {
                Exception exception = null;

                try
                {
                    var message = DomainUtility.GetSimpleMessageWithRecipientById();
                    const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                    var messageContent = new StringContent(string.Empty);

                    SendMessage(message, new FakeResponseHandler {ResultCode = statusCode, HttpContent = messageContent});
                }
                catch (AggregateException e)
                {
                    exception = e.InnerExceptions.ElementAt(0);
                }
                catch (ClientResponseException e)
                {
                    exception = e;
                }

                Assert.True(exception is ClientResponseException);
            }

            [Fact]
            public void ProperRequestSentRecipientById()
            {
                var message = DomainUtility.GetSimpleMessageWithRecipientById();
                SendMessage(message, new FakeResponseHandler {ResultCode = HttpStatusCode.OK, HttpContent = XmlResource.SendMessage.GetMessageDelivery()});
            }

            [Fact]
            public void ProperRequestSentRecipientByNameAndAddress()
            {
                var message = DomainUtility.GetSimpleMessageWithRecipientByNameAndAddress();

                SendMessage(message, new FakeResponseHandler {ResultCode = HttpStatusCode.OK, HttpContent = XmlResource.SendMessage.GetMessageDelivery()});
            }

            [Fact]
            public void ShouldSerializeErrorMessage()
            {
                Exception exception = null;

                try
                {
                    var message = DomainUtility.GetSimpleMessageWithRecipientById();
                    const HttpStatusCode statusCode = HttpStatusCode.NotFound;
                    var messageContent = XmlResource.SendMessage.GetError();

                    SendMessage(message, new FakeResponseHandler {ResultCode = statusCode, HttpContent = messageContent});
                }
                catch (AggregateException e)
                {
                    exception = e.InnerExceptions.ElementAt(0);
                }
                catch (ClientResponseException e)
                {
                    exception = e;
                }

                Assert.True(exception is ClientResponseException);
            }
        }

        public class SendIdentifyMethod : DigipostApiIntegrationTests
        {
            private void Identify(IIdentification identification)
            {
                var fakeResponseHandler = new FakeResponseHandler {ResultCode = HttpStatusCode.OK, HttpContent = XmlResource.Identification.GetResult()};
                var digipostApi = GetDigipostApi(fakeResponseHandler);

                digipostApi.Identify(identification);
            }

            [Fact]
            public void ProperRequestSent()
            {
                var identification = DomainUtility.GetPersonalIdentification();
                Identify(identification);
            }
        }
    }
}
