using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;

namespace Digipost.Api.Client.Handlers
{
    internal class LoggingHandler : DelegatingHandler
    {
        private ClientConfig ClientConfig { get; set; }

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public LoggingHandler(HttpMessageHandler innerHandler, ClientConfig clientConfig)
            : base(innerHandler)
        {
            ClientConfig = clientConfig;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (Log.IsDebugEnabled)
            {
                await LogRequest(request);
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (Log.IsDebugEnabled)
            {
                await LogResponse(response);
            }

            return response;
        }

        private async Task LogRequest(HttpRequestMessage request)
        {
            LogContent(request.Content, isRequest: true);
        }

        private async Task LogResponse(HttpResponseMessage response)
        {
            await LogContent(response.Content, false);
        }

        private async Task LogContent(HttpContent httpContent, bool isRequest)
        {
            var logPrefix = isRequest ? "Outgoing" : "Incoming";

            if (Log.IsDebugEnabled && ClientConfig.LogRequestAndResponse && httpContent != null)
            {
                var data = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
                Log.Debug($"{logPrefix} {data}");
            }
        }
    }
}