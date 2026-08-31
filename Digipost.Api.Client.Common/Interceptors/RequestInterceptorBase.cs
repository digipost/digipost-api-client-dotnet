using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Common
{
    /// <summary>
    ///     Convenience base class for <see cref="IRequestInterceptor" /> that provides no-op default implementations.
    ///     Override only the phase(s) you care about.
    /// </summary>
    public abstract class RequestInterceptorBase : IRequestInterceptor
    {
        public virtual Task OnBeforeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public virtual Task OnAfterResponseAsync(HttpResponseMessage response, HttpRequestMessage request, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
