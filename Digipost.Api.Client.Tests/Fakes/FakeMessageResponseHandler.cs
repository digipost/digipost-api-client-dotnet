using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Tests.Mocks
{
    public class FakeMessageResponseHandler : DelegatingHandler
    {
        public int HasBeenCalled = 0;
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = MessageContent(),
                StatusCode = HttpStatusCode.OK
            };
            HasBeenCalled++;
            return await Task.FromResult(response);
        }
        
        private static HttpContent MessageContent()
        {
            return new StringContent(
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<message-delivery xmlns=\"http://api.digipost.no/schema/v6\">" +
                    "<delivery-method>DIGIPOST</delivery-method>" +
                    "<status>DELIVERED</status>" +
                    "<delivery-time>2015-05-19T17:15:54.672+02:00</delivery-time>" +
                    "<primary-document>" +
                        "<uuid>c7794fba-c2ac-4941-847c-71258fa6a060</uuid>" +
                        "<subject>Primary document</subject>" +
                        "<file-type>txt</file-type>" +
                        "<authentication-level>PASSWORD</authentication-level>" +
                        "<sensitivity-level>NORMAL</sensitivity-level>" +
                        "<content-hash hash-algorithm=\"SHA256\">5o0RMsXcgSZpGsL7FAmhSQnvGkqgOcvl5JDtMhXBSlc=</content-hash>" +
                    "</primary-document>" +
                    "<attachment>" +
                        "<uuid>ee3a5a8a-0649-4627-88bf-93028e8ece3d</uuid>" +
                        "<subject>Attachment</subject>" +
                        "<file-type>txt</file-type>" +
                        "<authentication-level>PASSWORD</authentication-level>" +
                        "<sensitivity-level>NORMAL</sensitivity-level>" +
                        "<content-hash hash-algorithm=\"SHA256\">S1S0f3BSFeH0wLkFQaUpIenvGPURNj1clkcFyyY/fXA=</content-hash>" +
                    "</attachment>" +
                "</message-delivery>");
        }
    }
}
