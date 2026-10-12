using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;

namespace Digipost.Api.Client.Internal
{
    public class InterceptorHandler : DelegatingHandler
    {
        private readonly IList<IRequestInterceptor> _interceptors;

        public InterceptorHandler(IList<IRequestInterceptor> interceptors)
        {
            _interceptors = interceptors ?? new List<IRequestInterceptor>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Pre-request: FIFO order
            foreach (var interceptor in _interceptors)
            {
                await interceptor.OnBeforeRequestAsync(request, cancellationToken).ConfigureAwait(false);
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            // Post-response: LIFO order
            for (var i = _interceptors.Count - 1; i >= 0; i--)
            {
                await _interceptors[i].OnAfterResponseAsync(response, request, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }
    }
}
