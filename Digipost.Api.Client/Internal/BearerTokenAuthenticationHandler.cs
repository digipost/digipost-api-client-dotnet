using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;

namespace Digipost.Api.Client.Internal
{
    internal class BearerTokenAuthenticationHandler : DelegatingHandler
    {
        public BearerTokenAuthenticationHandler(ClientConfig clientConfig, TokenProvider tokenProvider)
        {
            ClientConfig = clientConfig;
            TokenProvider = tokenProvider;
        }

        private ClientConfig ClientConfig { get; }

        private TokenProvider TokenProvider { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestHeaderUtility.ApplyCommonHeaders(request, ClientConfig.Broker.Id);

            var hashTask = RequestHeaderUtility.ApplyContentHashHeaderIfPresent(request);
            var tokenTask = TokenProvider.GetTokenAsync(cancellationToken);
            await Task.WhenAll(hashTask, tokenTask).ConfigureAwait(false);

            var accessToken = tokenTask.Result;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            try
            {
                TokenProvider.InvalidateToken(accessToken);

                var retryRequest = await CloneRequestAsync(request).ConfigureAwait(false);
                var refreshedToken = await TokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(false);
                retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshedToken);

                return await base.SendAsync(retryRequest, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                response.Dispose();
            }
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri) {Version = request.Version};

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (request.Content != null)
            {
                var contentBytes = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                var clonedContent = new ByteArrayContent(contentBytes);
                foreach (var header in request.Content.Headers)
                {
                    clonedContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                clone.Content = clonedContent;
            }

            return clone;
        }
    }
}
