using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common;

namespace Digipost.Api.Client.Docs
{
    public class CertificateLoading
    {
        public void LoadCertificateFromThumbprint()
        {
            var config = new ClientConfig(senderId: "xxxxx", environment: Environment.Production);
            var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");
        }

        public void LoadCertificateFrom()
        {
            var config = new ClientConfig(senderId: "xxxxx", environment: Environment.Production);
            var businessCertificate =
                new X509Certificate2(
                    @"C:\Path\To\Certificate\Cert.p12",
                    "secretPasswordProperlyInstalledAndLoaded",
                    X509KeyStorageFlags.Exportable
                );

            var client = new DigipostClient(config, businessCertificate);
        }
    }
}