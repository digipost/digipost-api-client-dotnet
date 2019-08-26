using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Tests.Fakes
{
    public class FakeResponseHandler : DelegatingHandler
    {
        public HttpStatusCode? ResultCode { get; set; }

        public HttpContent HttpContent { get; set; }

        private KeyValuePair<string, string> HttpResponseHeader { get; set; }


        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = HttpContent ?? HttpContent,
                StatusCode = ResultCode ?? HttpStatusCode.OK
            };
            return await Task.FromResult(response).ConfigureAwait(false);
        }

        private void AddResponseHeader(HttpResponseMessage response)
        {
            if (HttpResponseHeader.Key != null)
            {
                response.Headers.Add(HttpResponseHeader.Key, HttpResponseHeader.Value);
            }
        }
    }
}
