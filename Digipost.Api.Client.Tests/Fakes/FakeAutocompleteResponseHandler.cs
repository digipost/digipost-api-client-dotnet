using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Tests.Fakes
{
    internal class FakeAutocompleteResponseHandler : DelegatingHandler
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
                "<autocomplete xmlns=\"http://api.digipost.no/schema/v6\">" +
                    "<suggestion>" +
                        "<search-string>marit johansen</search-string>" +
                        "<link rel=\"https://qa2.api.digipost.no/relations/search\" uri=\"https://qa2.api.digipost.no/recipients/search/marit%20johansen\" media-type=\"application/vnd.digipost-v6+xml\"/>" +
                    "</suggestion>" +
                    "<suggestion>" +
                        "<search-string>marit bakken</search-string> " +
                        "<link rel=\"https://qa2.api.digipost.no/relations/search\" uri=\"https://qa2.api.digipost.no/recipients/search/marit%20bakken\" media-type=\"application/vnd.digipost-v6+xml\"/>" +
                    "</suggestion>" +
                    "<link rel=\"https://qa2.api.digipost.no/relations/self\" uri=\"https://qa2.api.digipost.no/recipients/suggest/Marit/\" media-type=\"application/vnd.digipost-v6+xml\"/>" +
                "</autocomplete>");             
        }                       
    }
}
