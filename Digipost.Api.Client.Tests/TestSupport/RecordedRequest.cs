using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Digipost.Api.Client.Tests.TestSupport
{
    internal sealed class RecordedRequest
    {
        public string Method { get; set; }

        public string Path { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public string Body { get; set; }

        public X509Certificate2 ClientCertificate { get; set; }
    }
}
