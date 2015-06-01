using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Handlers;

namespace Digipost.Api.Client.Action
{
    public abstract class DigipostAction
    {
        private readonly string _uri;
        private HttpClient _httpClient;

        public ClientConfig ClientConfig { get; set; }

        public X509Certificate2 BusinessCertificate { get; set; }
        public XmlDocument RequestContent { get; internal set; }

        internal HttpClient HttpClient
        {
            get
            {
                if (_httpClient != null) return _httpClient;

                Logging.Log(TraceEventType.Information, " - Initializing HttpClient");
                var loggingHandler = new LoggingHandler(new HttpClientHandler());
                var authenticationHandler = new AuthenticationHandler(ClientConfig, BusinessCertificate, _uri,
                    loggingHandler);

                _httpClient = new HttpClient(authenticationHandler)
                {
                    Timeout = TimeSpan.FromMilliseconds(ClientConfig.TimeoutMilliseconds),
                    BaseAddress = new Uri(ClientConfig.ApiUrl.AbsoluteUri)
                };

                return _httpClient;
            }
            set { _httpClient = value; }
        }

        protected DigipostAction(RequestContent requestContent, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            InitializeRequestXmlContent(requestContent);
            _uri = uri;
            ClientConfig = clientConfig;
            BusinessCertificate = businessCertificate;
        }

        public Task<HttpResponseMessage> SendAsync(RequestContent requestContent)
        {
            try
            {
                Logging.Log(TraceEventType.Information, " - Sending request.");
                return HttpClient.PostAsync(_uri, Content(requestContent));
            }
            finally
            {
                Logging.Log(TraceEventType.Information, " - Request sent.");
            }
        }

        protected abstract HttpContent Content(RequestContent requestContent);

        protected abstract string Serialize(RequestContent requestContent);

        private void InitializeRequestXmlContent(RequestContent requestContent)
        {
            var document = new XmlDocument();

            if (requestContent == null)
            {
                throw new ConfigException("Null or empty Request content" + typeof(RequestContent) + ".  Digipost Action not configured correctly.");
            }

            var serialized = Serialize(requestContent);
            document.LoadXml(serialized);

            RequestContent = document;
        }
    }
}