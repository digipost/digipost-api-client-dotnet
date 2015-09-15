using System.Net.Http;

namespace Digipost.Api.Client.Tests.Fakes
{
    public class FakeHttpClientHandlerForIdentificationResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StringContent(
                 "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<identification-result xmlns=\"http://api.digipost.no/schema/v6\">" +
                    "<result>DIGIPOST</result>" +
                "</identification-result>");
        }
    }
}
