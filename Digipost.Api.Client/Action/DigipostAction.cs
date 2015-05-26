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
        private readonly RequestContent _content;
        private readonly string _uri;
        private HttpClient _httpClient;
        
        public ClientConfig ClientConfig { get; set; }
        
        public X509Certificate2 BusinessCertificate { get; set; }

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

        protected DigipostAction(RequestContent content, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            _content = content;
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

        private XmlDocument _requestContent;

        public RequestContent RequestContent
        {
            get
            {
                if(_requestContent == null || _requestContent.InnerXml.Length == 0)
                    throw new ConfigException("Null or empty exception.  Digipost Action not configured correctly.");

                return null;
            }
        }
        
    }
}