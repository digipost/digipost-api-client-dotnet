using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Shared.Certificate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Digipost.Api.Client
{
    /// <summary>
    ///     Fluent builder for creating a <see cref="DigipostClient" /> with optional interceptors and logging.
    /// </summary>
    public class DigipostClientBuilder
    {
        private readonly ClientConfig _clientConfig;
        private readonly X509Certificate2 _certificate;
        private ILoggerFactory _loggerFactory = new NullLoggerFactory();
        private readonly List<IRequestInterceptor> _interceptors = new List<IRequestInterceptor>();

        public DigipostClientBuilder(ClientConfig clientConfig, X509Certificate2 certificate)
        {
            _clientConfig = clientConfig;
            _certificate = certificate;
        }

        public DigipostClientBuilder(ClientConfig clientConfig, string thumbprint)
            : this(clientConfig, CertificateUtility.SenderCertificate(thumbprint))
        {
        }

        /// <summary>
        ///     Configures the client to use the provided <see cref="ILoggerFactory" /> for logging.
        /// </summary>
        public DigipostClientBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        /// <summary>
        ///     Adds a request interceptor to the HTTP pipeline.
        ///     Interceptors are invoked in the order they are added (FIFO for pre-request, LIFO for post-response).
        /// </summary>
        public DigipostClientBuilder AddInterceptor(IRequestInterceptor interceptor)
        {
            _interceptors.Add(interceptor);
            return this;
        }

        /// <summary>
        ///     Builds and returns a configured <see cref="DigipostClient" />.
        /// </summary>
        public DigipostClient Build()
        {
            return new DigipostClient(_clientConfig, _certificate, _loggerFactory, _interceptors);
        }
    }
}
