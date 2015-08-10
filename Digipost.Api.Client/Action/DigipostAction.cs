using System;
using System.Diagnostics;
using System.Net;
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

        private readonly object _threadLock = new object();

        protected DigipostAction(RequestContent requestContent, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            InitializeRequestXmlContent(requestContent);
            _uri = uri;
            ClientConfig = clientConfig;
            BusinessCertificate = businessCertificate;
        }

        internal HttpClient ThreadSafeHttpClient
        {
            get
            {
                lock (_threadLock)
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
            }

            set
            {
                lock (_threadLock)
                {
                    _httpClient = value;
                }
            }
        }

        internal Task<HttpResponseMessage> PostAsync(RequestContent requestContent)
        {
            try
            {
                Logging.Log(TraceEventType.Information, " - Sending request (POST).");
                return ThreadSafeHttpClient.PostAsync(_uri, Content(requestContent));
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
                return ThreadSafeHttpClient.GetAsync(_uri);
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
            if (requestContent == null) return;

            var document = new XmlDocument();
            var serialized = Serialize(requestContent);
            document.LoadXml(serialized);
            RequestContent = document;
        }
    }
}