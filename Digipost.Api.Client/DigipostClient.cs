using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Common.SenderInfo;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Internal;
using Digipost.Api.Client.Send;
using Digipost.Api.Client.Shared.Certificate;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using V8 = Digipost.Api.Client.Common.Generated.V8;

namespace Digipost.Api.Client
{
    public class DigipostClient
    {
        private readonly ClientConfig _clientConfig;
        private readonly RequestHelper _requestHelper;
        private readonly IMemoryCache _entrypointCache;

        private readonly ILogger<DigipostClient> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public DigipostClient(ClientConfig clientConfig, string thumbprint)
            : this(clientConfig, CertificateUtility.SenderCertificate(thumbprint), new NullLoggerFactory())
        {
        }

        public DigipostClient(ClientConfig clientConfig, X509Certificate2 enterpriseCertificate)
            : this(clientConfig, enterpriseCertificate, new NullLoggerFactory())
        {
        }

        public DigipostClient(ClientConfig clientConfig, X509Certificate2 enterpriseCertificate, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DigipostClient>();
            _loggerFactory = loggerFactory;
            _entrypointCache = new MemoryCache(new MemoryCacheOptions());

            _clientConfig = clientConfig;
            var httpClient = GetHttpClient(enterpriseCertificate, clientConfig.WebProxy, clientConfig.Credential);
            _requestHelper = new RequestHelper(httpClient, _loggerFactory);
        }

        private SendMessageApi _sendMessageApi()
        {
            return new SendMessageApi(new SendRequestHelper(_requestHelper), _loggerFactory, GetRoot(new ApiRootUri()));
        }

        private HttpClient GetHttpClient(X509Certificate2 enterpriseCertificate, WebProxy proxy = null, NetworkCredential credential = null)
        {
            var allDelegationHandlers = new List<DelegatingHandler> {new LoggingHandler(_clientConfig, _loggerFactory), new AuthenticationHandler(_clientConfig, enterpriseCertificate, _loggerFactory)};

            var httpMessageHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            if (proxy != null)
            {
                proxy.Credentials = credential;
                httpMessageHandler.Proxy = proxy;
                httpMessageHandler.UseProxy = true;
                httpMessageHandler.UseDefaultCredentials = false;
            }
            var httpClient = HttpClientFactory.Create(
                httpMessageHandler,
                allDelegationHandlers.ToArray()
            );

            httpClient.Timeout = TimeSpan.FromMilliseconds(_clientConfig.TimeoutMilliseconds);
            httpClient.BaseAddress = new Uri(_clientConfig.Environment.Url.AbsoluteUri);

            return httpClient;
        }

        public Root GetRoot(ApiRootUri apiRootUri)
        {
            var cacheKey = "root" + apiRootUri;

            if (_entrypointCache.TryGetValue(cacheKey, out Root root)) return root;

            var result = _requestHelper.Get<V8.Entrypoint>(apiRootUri).ConfigureAwait(false);
            var entrypoint = result.GetAwaiter().GetResult();

            root = entrypoint.FromDataTransferObject();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for 5 minutes when in use, but max 1 hour.
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            return _entrypointCache.Set(cacheKey, root, cacheEntryOptions);
        }

        /// <summary>
        /// Fetch Sender Information
        /// </summary>
        /// <param name="sender">The sender is optional. If not specified, the broker will be used.</param>
        /// <returns></returns>
        public SenderInformation GetSenderInformation(Sender sender = null)
        {
            var senderToUse = sender ?? new Sender(_clientConfig.Broker.Id);
            var senderInformationUri = GetRoot(new ApiRootUri()).GetSenderInformationUri(senderToUse);

            return new SenderInformationApi(_entrypointCache, _requestHelper).GetSenderInformation(senderInformationUri);
        }

        public SenderInformation GetSenderInformation(SenderOrganisation senderOrganisation)
        {
            var senderInformationUri = GetRoot(new ApiRootUri()).GetSenderInformationUri(senderOrganisation.OrganisationNumber, senderOrganisation.PartId);

            return new SenderInformationApi(_entrypointCache, _requestHelper).GetSenderInformation(senderInformationUri);
        }

        public Inbox.Inbox GetInbox(Sender senderId)
        {
            return new Inbox.Inbox(_requestHelper, GetRoot(new ApiRootUri(senderId)));
        }

        public Archive.IArchiveApi GetArchive(Sender senderId = null)
        {
            return new Archive.ArchiveApi(_requestHelper, _loggerFactory, GetRoot(new ApiRootUri(senderId)));
        }

        /// <summary>
        /// Get access to the document api.
        /// </summary>
        /// <param name="sender">Optional parameter for sender if you are a broker. If you don't specify a sender, your broker ident will be used</param>
        /// <returns></returns>
        public IDocumentsApi DocumentsApi(Sender sender = null)
        {
            var senderToUse = sender ?? new Sender(_clientConfig.Broker.Id);
            return new DocumentsApi(_requestHelper, _loggerFactory, GetRoot(new ApiRootUri(sender)), senderToUse);
        }

        public IDocumentsApi GetDocumentApi(Sender sender = null)
        {
            return new DocumentsApi(_requestHelper, _loggerFactory, GetRoot(new ApiRootUri(sender)), sender);
        }
        public IShareDocumentsApi GetDocumentSharing(Sender sender = null)
        {
            return new DocumentsApi(_requestHelper, _loggerFactory, GetRoot(new ApiRootUri(sender)), sender);
        }

        public IIdentificationResult Identify(IIdentification identification)
        {
            return _sendMessageApi().Identify(identification);
        }

        public Task<IIdentificationResult> IdentifyAsync(IIdentification identification)
        {
            return _sendMessageApi().IdentifyAsync(identification);
        }

        public IMessageDeliveryResult SendMessage(IMessage message)
        {
            return _sendMessageApi().SendMessage(message, _clientConfig.SkipMetaDataValidation);
        }

        public Task<IMessageDeliveryResult> SendMessageAsync(IMessage message)
        {
            return _sendMessageApi().SendMessageAsync(message, _clientConfig.SkipMetaDataValidation);
        }

        public void AddAdditionalData(AdditionalData additionalData, AddAdditionalDataUri uri)
        {
            _sendMessageApi().SendAdditionalData(additionalData, uri);
        }

        public ISearchDetailsResult Search(string query)
        {
            return _sendMessageApi().Search(query);
        }

        public Task<ISearchDetailsResult> SearchAsync(string query)
        {
            return _sendMessageApi().SearchAsync(query);
        }
    }
}
