using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Tests.Mocks
{
    public class FakeIdentificationResponseHandler : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await new Task<HttpResponseMessage>(() => GetResponse());
        }

        private HttpResponseMessage GetResponse()
        {
            var responseMessage = new HttpResponseMessage();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = MessageContent();
            return responseMessage;
        }

        private HttpContent MessageContent()
        {
            return new StringContent(
                 "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<identification-result xmlns=\"http://api.digipost.no/schema/v6\">" +
                    "<result>DIGIPOST</result>" +
                "</identification-result>");
        }
    }
}
