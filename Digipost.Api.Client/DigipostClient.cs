using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Exceptions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Send;
using Digipost.Api.Client.Shared.Certificate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Digipost.Api.Client
{
    public class DigipostClient
    {
        private readonly SendMessageApi _api;
        private readonly ClientConfig _clientConfig;
        private readonly RequestHelper _requestHelper;
        
        private HttpClient _httpClient;
        private readonly ILogger<DigipostClient> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public DigipostClient(ClientConfig clientConfig, string thumbprint)
            : this(clientConfig, CertificateUtility.SenderCertificate(thumbprint), new NullLoggerFactory())
        {
        }

        public DigipostClient(ClientConfig clientConfig, X509Certificate2 businessCertificate, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DigipostClient>();
            _loggerFactory = loggerFactory;
            
            _clientConfig = clientConfig;
            _httpClient = GetHttpClient(businessCertificate);
            _requestHelper = new RequestHelper(_httpClient, _loggerFactory);
            _api = new SendMessageApi(new SendRequestHelper(_requestHelper));
        }

        private HttpClient GetHttpClient(X509Certificate2 businessCertificate)
        {
            HttpClientHandler handler = new HttpClientHandler();
            var clientCertificates = new X509Certificate2Collection {businessCertificate};
            handler.ClientCertificates.AddRange(clientCertificates);
            handler.ServerCertificateCustomValidationCallback = ValidateServerCertificateThrowIfInvalid;
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var loggingHandler = new LoggingHandler(_clientConfig, _loggerFactory);
            var authenticationHandler = new AuthenticationHandler(_clientConfig, businessCertificate);

            var httpClient = HttpClientFactory.Create(
                handler,
                authenticationHandler,
                loggingHandler
            );

            httpClient.Timeout = TimeSpan.FromMilliseconds(_clientConfig.TimeoutMilliseconds);
            httpClient.BaseAddress = new Uri(_clientConfig.Environment.Url.AbsoluteUri);
            
            return httpClient;
        }

        public Inbox.Inbox GetInbox(Sender senderId)
        {
            return new Inbox.Inbox(senderId, _requestHelper);
        }

        public IIdentificationResult Identify(IIdentification identification)
        {
            return _api.Identify(identification);
        }

        public Task<IIdentificationResult> IdentifyAsync(IIdentification identification)
        {
            return _api.IdentifyAsync(identification);
        }

        public IMessageDeliveryResult SendMessage(IMessage message)
        {
            return _api.SendMessage(message);
        }

        public Task<IMessageDeliveryResult> SendMessageAsync(IMessage message)
        {
            return _api.SendMessageAsync(message);
        }

        public ISearchDetailsResult Search(string query)
        {
            return _api.Search(query);
        }

        public Task<ISearchDetailsResult> SearchAsync(string query)
        {
            return _api.SearchAsync(query);
        }
        
        private bool ValidateServerCertificateThrowIfInvalid(HttpRequestMessage message, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            var x509Certificate2 = new X509Certificate2(certificate);

            var validationResult = CertificateValidator.ValidateCertificate(x509Certificate2);

            if (validationResult.Type != CertificateValidationType.Valid)
            {
                _logger.LogError($"Certificate received in the response is not valid. The reason is '{validationResult.Type}', description: '{validationResult.Message}'", null);
                throw new CertificateException($"Certificate received in the response is not valid. The reason is '{validationResult.Type}', description: '{validationResult.Message}'", null);
            }

            return true;
        }
    }
}
