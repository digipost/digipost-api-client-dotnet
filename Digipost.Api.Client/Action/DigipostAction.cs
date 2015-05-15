using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Handlers;

namespace Digipost.Api.Client.Action
{
    internal abstract class DigipostAction
    {
        private readonly string _uri;

        protected DigipostAction(ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            _uri = uri;
            ClientConfig = clientConfig;
            BusinessCertificate = businessCertificate;
        }

        public ClientConfig ClientConfig { get; set; }
        public X509Certificate2 BusinessCertificate { get; set; }
        protected abstract HttpContent Content(RequestContent requestContent);

        public Task<HttpResponseMessage> SendAsync(RequestContent requestContent)
        {
            Logging.Log(TraceEventType.Information, "> Starting to build request ...");
            var loggingHandler = new LoggingHandler(new HttpClientHandler());
            var authenticationHandler = new AuthenticationHandler(ClientConfig, BusinessCertificate, _uri, loggingHandler);

            Logging.Log(TraceEventType.Information, " - Initializing HttpClient");
            var client = new HttpClient(authenticationHandler);

            Logging.Log(TraceEventType.Information, " - Sending request.");
            client.Timeout = TimeSpan.FromMilliseconds(ClientConfig.TimeoutMilliseconds);
            client.BaseAddress = new Uri(ClientConfig.ApiUrl.AbsoluteUri);

            Logging.Log(TraceEventType.Information, " - Request sent.");

            return client.PostAsync(_uri, Content(requestContent));
        }
    }
}