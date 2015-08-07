using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Tests.Fakes
{
    internal class FakePersonDetailsResponseHandler: DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = MessageContent(),
                StatusCode = HttpStatusCode.OK
            };
            return await Task.FromResult(response);
        }

        private static HttpContent MessageContent()
        {
            throw new NotImplementedException();
        }
    }
}
