using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Digipost.Api.Client.Tests.TestSupport
{
    internal static class SelfSignedCertificateFactory
    {
        public static X509Certificate2 CreateSelfSigned(string subjectName = "CN=digipost-mtls-test")
        {
            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(subjectName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, false));

                request.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(
                        new OidCollection
                        {
                            new Oid("1.3.6.1.5.5.7.3.1"), // Server Authentication
                            new Oid("1.3.6.1.5.5.7.3.2") // Client Authentication
                        },
                        false));

                var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddHours(1));

                return X509CertificateLoader.LoadPkcs12(certificate.Export(X509ContentType.Pfx), null, X509KeyStorageFlags.Exportable);
            }
        }
    }
}
