using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;

namespace Digipost.Api.Client.Test.Utilities
{
    internal class SenderUtility
    {
        public static Sender GetSender(Environment environment)
        {
            switch (environment)
            {
                case Environment.DifiTest:
                    return new Sender(
                        "497013",
                        "‎2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed", //Digipost Testintegrasjon for Digital Post
                        "https://qaoffentlig.api.digipost.no/"
                        );
                case Environment.Qa:
                    return new Sender(
                        "779051",
                        "d8 6e 19 1b 8f 9b 0b 57 3e db 72 db a8 09 1f dc 6a 10 18 fd", //DNB
                        "https://qa.api.digipost.no/"
                        );
                default:
                    throw new ArgumentOutOfRangeException(nameof(environment), environment, null);
            }
        }
    }

    internal enum Environment
    {
        DifiTest,
        Qa
    }

    internal class Sender
    {
        public Sender(string id, string certificateThumbprint, string environment)
        {
            Id = id;
            Certificate = CertificateUtility.SenderCertificate(certificateThumbprint, Language.English);
            Environment = environment;
        }

        public string Id { get; set; }

        public string Environment { get; set; }

        public X509Certificate2 Certificate { get; set; }
    }
}