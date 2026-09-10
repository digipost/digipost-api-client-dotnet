using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;

namespace Digipost.Api.Client.Common
{
    /// <summary>
    ///     Contains configuration for authenticating with the Digipost API using JWT/mTLS (OAuth 2.0 client credentials).
    ///     This is the recommended authentication method - see <see cref="ClientConfig" /> for the rest of the client setup.
    /// </summary>
    public class JwtAuthConfig
    {
        /// <summary>
        ///     Configuration for JWT/mTLS authentication.
        /// </summary>
        /// <param name="clientId">The OAuth2 client id registered for your enterprise certificate at https://nyva.digipost.no .</param>
        /// <param name="thumbprint">
        ///     The thumbprint of the enterprise certificate to present over mTLS, as installed in the OS certificate store.
        /// </param>
        public JwtAuthConfig(string clientId, string thumbprint)
            : this(clientId, CertificateUtility.SenderCertificate(thumbprint))
        {
        }

        /// <summary>
        ///     Configuration for JWT/mTLS authentication, loading the enterprise certificate directly rather than from the
        ///     OS certificate store - for instance with <c>X509CertificateLoader.LoadPkcs12FromFile(path, password)</c>.
        /// </summary>
        /// <param name="clientId">The OAuth2 client id registered for your enterprise certificate at https://nyva.digipost.no .</param>
        /// <param name="enterpriseCertificate">The enterprise certificate to present over mTLS when requesting an access token.</param>
        public JwtAuthConfig(string clientId, X509Certificate2 enterpriseCertificate)
        {
            ClientId = clientId;
            EnterpriseCertificate = enterpriseCertificate;
        }

        /// <summary>
        ///     The OAuth2 client id registered for your enterprise certificate at https://nyva.digipost.no .
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     The enterprise certificate presented over mTLS to mIDP when requesting an access token.
        ///     Only read once, when DigipostClient is constructed - reassigning it afterwards has no effect on an
        ///     already-constructed client.
        /// </summary>
        public X509Certificate2 EnterpriseCertificate { get; set; }

        /// <summary>
        ///     The mIDP token endpoint to request access tokens from. Derived automatically from
        ///     <see cref="ClientConfig.Environment" /> unless explicitly set here, which is only needed for a custom/on-prem
        ///     setup.
        /// </summary>
        public Uri TokenEndpoint { get; set; }
    }
}
