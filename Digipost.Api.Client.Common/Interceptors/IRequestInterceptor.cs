using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Common
{
    /// <summary>
    ///     Allows hooking into the pre-request and post-response phases of every HTTP call made by the client.
    /// </summary>
    public interface IRequestInterceptor
    {
        /// <summary>
        ///     Called before the request is sent. Can modify the request.
        /// </summary>
        Task OnBeforeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Called after the response is received. Can inspect or modify the response.
        /// </summary>
        Task OnAfterResponseAsync(HttpResponseMessage response, HttpRequestMessage request, CancellationToken cancellationToken = default);
    }
}
