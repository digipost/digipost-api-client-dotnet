using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Digipost.Api.Client.Common.Exceptions;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Resources.Xml;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Digipost.Api.Client.Inbox.Tests.Integration
{
    public class InboxApiIntegrationTests
    {
        private readonly Inbox _inbox = GetInbox();

        private static Inbox GetInbox()
        {
            var httpClient = new HttpClient(
                new FakeResponseHandler {ResultCode = HttpStatusCode.NotFound, HttpContent = XmlResource.Inbox.GetError()}
            )
            {
                BaseAddress = new Uri("http://www.fakeBaseAddress.no")
            };
            var requestHelper = new RequestHelper(httpClient, new NullLoggerFactory());

            var inbox = new Inbox(DomainUtility.GetSender(), requestHelper);
            return inbox;
        }

        public class FetchMethod : InboxApiIntegrationTests
        {
            [Fact]
            public void ErrorShouldCauseDigipostResponseException()
            {
                Exception exception = null;
                try
                {
                    _inbox.Fetch().Wait();
                }
                catch (AggregateException e)
                {
                    exception = e.InnerExceptions.ElementAt(0);
                }

                Assert.True(exception?.GetType() == typeof(ClientResponseException));
            }
        }

        public class FetchDocumentMethod : InboxApiIntegrationTests
        {
            [Fact]
            public void ErrorShouldCauseDigipostResponseException()
            {
                Exception exception = null;
                try
                {
                    _inbox.FetchDocument(new InboxDocument()).Wait();
                }
                catch (AggregateException e)
                {
                    exception = e.InnerExceptions.ElementAt(0);
                }

                Assert.True(exception?.GetType() == typeof(ClientResponseException));
            }
        }

        public class DeleteDocumentMethod : InboxApiIntegrationTests
        {
            [Fact]
            public void ErrorShouldCauseDigipostResponseException()
            {
                Exception exception = null;
                try
                {
                    _inbox.DeleteDocument(new InboxDocument()).Wait();
                }
                catch (AggregateException e)
                {
                    exception = e.InnerExceptions.ElementAt(0);
                }

                Assert.True(exception?.GetType() == typeof(ClientResponseException));
            }
        }
    }
}
