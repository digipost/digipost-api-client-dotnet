using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Handlers;

namespace Digipost.Api.Client.Action
{
    public abstract class DigipostAction
    {
        private string Uri { get; }

        public ClientConfig ClientConfig { get; set; }

        public X509Certificate2 BusinessCertificate { get; set; }

        public XmlDocument RequestContent { get; internal set; }

        protected DigipostAction(IRequestContent requestContent, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            InitializeRequestXmlContent(requestContent);
            Uri = uri;
            ClientConfig = clientConfig;
            BusinessCertificate = businessCertificate;
            ThreadSafeHttpClient = HttpClient();
        }

        private HttpClient HttpClient()
        {
            var loggingHandler = new LoggingHandler(new HttpClientHandler());
            var authenticationHandler = new AuthenticationHandler(ClientConfig, BusinessCertificate, Uri,
            loggingHandler);

            var httpClient = new HttpClient(authenticationHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(ClientConfig.TimeoutMilliseconds),
                BaseAddress = new Uri(ClientConfig.ApiUrl.AbsoluteUri)
            };

            return httpClient;
        }

        internal HttpClient ThreadSafeHttpClient { get; set; }

        internal Task<HttpResponseMessage> PostAsync(IRequestContent requestContent)
        {
            try
            {
                Logging.Log(TraceEventType.Information, " - Sending request (POST).");
                return ThreadSafeHttpClient.PostAsync(Uri, Content(requestContent));
            }
            finally
            {
                Logging.Log(TraceEventType.Information, " - Request sent.");
            }
        }

        internal Task<HttpResponseMessage> GetAsync()
        {
            try
            {
                Logging.Log(TraceEventType.Information, " - Sending request (GET).");
                return ThreadSafeHttpClient.GetAsync(Uri);
            }
            finally
            {
                Logging.Log(TraceEventType.Information, " - Request sent.");
            }
        }

        protected abstract HttpContent Content(IRequestContent requestContent);

        protected abstract string Serialize(IRequestContent requestContent);

        private void InitializeRequestXmlContent(IRequestContent requestContent)
        {
            if (requestContent == null) return;

            var document = new XmlDocument();
            var serialized = Serialize(requestContent);
            document.LoadXml(serialized);
            RequestContent = document;
        }
    }
}