using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;

namespace Digipost.Api.Client.Common.Handlers
{
    internal class LoggingHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public LoggingHandler(HttpMessageHandler innerHandler, ClientConfig clientConfig)
            : base(innerHandler)
        {
            ClientConfig = clientConfig;
        }

        private ClientConfig ClientConfig { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await LogRequest(request).ConfigureAwait(false);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            await LogResponse(response).ConfigureAwait(false);

            return response;
        }

        private async Task LogRequest(HttpRequestMessage request)
        {
            await LogContent(request.Content, true).ConfigureAwait(false);
        }

        private async Task LogResponse(HttpResponseMessage response)
        {
            await LogContent(response.Content, false).ConfigureAwait(false);
        }

        private async Task LogContent(HttpContent httpContent, bool isRequest)
        {
            var logPrefix = isRequest ? "Outgoing" : "Incoming";

            if (Log.IsDebugEnabled && ClientConfig.LogRequestAndResponse && (httpContent != null))
            {
                var data = httpContent.ReadAsStringAsync().ConfigureAwait(false);
                Log.Debug($"{logPrefix} {data}");
            }
        }
    }
}