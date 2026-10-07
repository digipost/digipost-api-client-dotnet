using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common;

namespace Digipost.Api.Client.Docs
{
    public class JwtAuthenticationExamples
    {
        private static readonly Broker broker = new Broker(12345);

        public void ClientConfigurationWithJwtMtlsAuthentication()
        {
            var clientConfig = new ClientConfig(broker, Environment.Production);

            var jwtAuthConfig = new JwtAuthConfig(clientId: "your-oauth-client-id", thumbprint: "84e492a972b7e...");

            var client = new DigipostClient(clientConfig, jwtAuthConfig);
        }

        public void ClientConfigurationWithJwtMtlsAuthenticationFromFile()
        {
            var clientConfig = new ClientConfig(broker, Environment.Production);

            var enterpriseCertificate =
                X509CertificateLoader.LoadPkcs12FromFile(
                    @"C:\Path\To\Certificate\Cert.p12",
                    "secretPasswordProperlyInstalledAndLoaded",
                    X509KeyStorageFlags.Exportable
                );

            var jwtAuthConfig = new JwtAuthConfig(clientId: "your-oauth-client-id", enterpriseCertificate);

            var client = new DigipostClient(clientConfig, jwtAuthConfig);
        }
    }
}
