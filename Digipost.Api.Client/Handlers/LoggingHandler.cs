using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Handlers
{
    internal class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Logging.Log(TraceEventType.Information, " LoggingHandler > sendAsync() - Start!");
            Logging.Log(TraceEventType.Information, " LoggingHandler > Request:" + request);

            if (request.Content != null)
            {
                Logging.Log(TraceEventType.Information, " LoggingHandler >  content:" + await request.Content.ReadAsStringAsync().ConfigureAwait(false));
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            Logging.Log(TraceEventType.Information, " LoggingHandler >  Response:" + response);

            if (response.Content != null)
            {
                Logging.Log(TraceEventType.Information, " LoggingHandler >  content:" + await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            Logging.Log(TraceEventType.Information, " LoggingHandler > sendAsync() - End!");
            return response;
        }
    }
}