using System.Net.Http;

namespace Digipost.Api.Client.Test.Fakes
{
    class FakeResponseHandler : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            throw new System.NotImplementedException("Content must always be set! This is a temporary class to discard pattern of wrapping all exceptions in its own class.");
        }
    }
}
