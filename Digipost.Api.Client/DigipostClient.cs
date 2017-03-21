using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Handlers;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client
{
    public class DigipostClient
    {
        private readonly DigipostApi _api;
        private readonly ClientConfig _clientConfig;
        private readonly RequestHelper _requestHelper;
        private readonly SendRequestHelper _sendRequestHelper;

        public DigipostClient(ClientConfig clientConfig, X509Certificate2 businessCertificate)
        {
            _clientConfig = clientConfig;
            _requestHelper = new RequestHelper(GetHttpClient(businessCertificate), clientConfig, businessCertificate);
            _sendRequestHelper = new SendRequestHelper(GetHttpClient(businessCertificate), clientConfig, businessCertificate);

            _api = new DigipostApi(clientConfig, businessCertificate, _requestHelper);
        }

        public DigipostClient(ClientConfig clientConfig, string thumbprint)
        {
            _api = new DigipostApi(clientConfig, thumbprint);
        }

        private HttpClient GetHttpClient(X509Certificate2 businessCertificate)
        {
            var loggingHandler = new LoggingHandler(
                new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate},
                _clientConfig
            );

            var authenticationHandler = new AuthenticationHandler(_clientConfig, businessCertificate, loggingHandler);

            var httpClient = new HttpClient(authenticationHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(_clientConfig.TimeoutMilliseconds),
                BaseAddress = new Uri(_clientConfig.Environment.Url.AbsoluteUri)
            };

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
            return _api.SendMessage(_sendRequestHelper, message);
        }

        public Task<IMessageDeliveryResult> SendMessageAsync(IMessage message)
        {
            return _api.SendMessageAsync(_sendRequestHelper, message);
        }

        public ISearchDetailsResult Search(string query)
        {
            return _api.Search(query);
        }

        public Task<ISearchDetailsResult> SearchAsync(string query)
        {
            return _api.SearchAsync(query);
        }
    }
}