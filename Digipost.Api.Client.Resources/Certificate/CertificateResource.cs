using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Api.Client.Resources.Certificate
{
    internal class CertificateResource
    {
        public static X509Certificate2 Certificate()
        {
            var resourceUtility = new ResourceUtility(typeof(CertificateResource).GetTypeInfo().Assembly, "Digipost.Api.Client.Resources.Certificate.Data");
            return new X509Certificate2(resourceUtility.ReadAllBytes("DigipostTestCert.p12"), string.Empty, X509KeyStorageFlags.Exportable);
        }
    }
}
