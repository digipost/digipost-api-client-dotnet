using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Tests.Mocks
{
    public class FakeHttpClientHandlerForIdentificationResponse : DelegatingHandler
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
            return new StringContent(
                 "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<identification-result xmlns=\"http://api.digipost.no/schema/v6\">" +
                    "<result>DIGIPOST</result>" +
                "</identification-result>");
        }
    }
}
