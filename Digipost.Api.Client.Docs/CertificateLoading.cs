using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common;

namespace Digipost.Api.Client.Docs
{
    public class CertificateLoading
    {
        private static readonly ClientConfig clientConfig = new ClientConfig(broker, Environment.Production);
        private static readonly DigipostClient client = new DigipostClient(clientConfig, thumbprint: "84e492a972b7e...");
        private static readonly Sender sender = new Sender(67890);
        private static readonly Broker broker = new Broker(12345);

        public void LoadCertificateFromThumbprint()
        {
            var clientConfig = new ClientConfig(broker, Environment.Production);
            var client = new DigipostClient(clientConfig, thumbprint: "84e492a972b7e...");
        }

        public void LoadCertificateFrom()
        {
            var clientConfig = new ClientConfig(broker, Environment.Production);
            var businessCertificate =
                new X509Certificate2(
                    @"C:\Path\To\Certificate\Cert.p12",
                    "secretPasswordProperlyInstalledAndLoaded",
                    X509KeyStorageFlags.Exportable
                );

            var client = new DigipostClient(clientConfig, businessCertificate);
        }
    }
}