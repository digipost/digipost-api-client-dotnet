using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;

namespace Digipost.Api.Client.Tests
{
    public static class TestProperties
    {

        public const string CertificatePassword = "abc123hest";

        public static X509Certificate2 Certificate()
        {
            var resourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");
            var certificate = new X509Certificate2(resourceUtility.ReadAllBytes(true, "DigipostCert.p12"), password: String.Empty, keyStorageFlags: X509KeyStorageFlags.Exportable);
            
            return certificate;
        }
    }
}
