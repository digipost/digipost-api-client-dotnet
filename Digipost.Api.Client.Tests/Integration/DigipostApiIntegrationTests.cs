using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Exceptions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Resources.Xml;
using Digipost.Api.Client.Send;
using Digipost.Api.Client.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Integration
{
    public class DigipostApiIntegrationTests
    {
        protected X509Certificate2 Certificate;
        protected ClientConfig ClientConfig;
        protected Uri Uri;

        // These tests will fail on MacOS due to how it deals with keys.
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

        internal HttpClient GetHttpClient(
            FakeResponseHandler fakehandler)
        {
            ClientConfig.LogRequestAndResponse = true;
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            
            var allDelegationHandlers = new List<DelegatingHandler> {new LoggingHandler(ClientConfig, serviceProvider.GetService<ILoggerFactory>()), new AuthenticationHandler(ClientConfig, Certificate, serviceProvider.GetService<ILoggerFactory>())};
            
            var httpClient = HttpClientFactory.Create(
                fakehandler,
                allDelegationHandlers.ToArray()
            );

            httpClient.BaseAddress = new Uri("http://www.fakeBaseAddress.no");
            
            return httpClient;
        }

        private SendMessageApi GetDigipostApi(FakeResponseHandler fakeResponseHandler)
        {
            var httpClient = GetHttpClient(fakeResponseHandler);
            
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            
            var requestHelper = new RequestHelper(httpClient, serviceProvider.GetService<ILoggerFactory>()) {HttpClient = httpClient};

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
