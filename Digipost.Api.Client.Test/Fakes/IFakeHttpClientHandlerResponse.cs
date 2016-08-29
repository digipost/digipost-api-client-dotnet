using System.Net;
using System.Net.Http;

namespace Digipost.Api.Client.Test.Fakes
{
    public interface IFakeHttpClientHandlerResponse
    {
        HttpStatusCode? ResultCode { get; set; }

        HttpContent HttpContent { get; set; }
    }
}