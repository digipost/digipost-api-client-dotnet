using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Recipient;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Utilities
{
    internal class SenderUtility
    {
        public static TestSender GetSender(TestEnvironment testEnvironment)
        {
            var digipostTestintegrasjonforDigitalPostThumbprint = "‎2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed";

            switch (testEnvironment)
            {
                case TestEnvironment.DifiTest:
                    return new TestSender(
                        497013,
                        digipostTestintegrasjonforDigitalPostThumbprint,
                        Environment.DifiTest,
                        new RecipientById(IdentificationType.DigipostAddress, "ReplaceMehere")
                    );
                case TestEnvironment.Qa:
                    return new TestSender(
                        1010,
                        digipostTestintegrasjonforDigitalPostThumbprint,
                        Environment.Qa,
                        new RecipientById(IdentificationType.DigipostAddress, "digipost.testintegrasjon.for.digita#VZJS")
                    );
                default:
                    throw new ArgumentOutOfRangeException(nameof(testEnvironment), testEnvironment, null);
            }
        }
    }

    internal enum TestEnvironment
    {
        DifiTest,
        Qa
    }

    internal class TestSender
    {
        public TestSender(long id, string certificateThumbprint, Environment environment, RecipientById recipientById)
        {
            Id = id;
            Certificate = CertificateUtility.SenderCertificate(certificateThumbprint, Language.English);
            Environment = environment;
            Recipient = recipientById;
        }

        public long Id { get; set; }

        public Environment Environment { get; set; }

        public X509Certificate2 Certificate { get; set; }

        public RecipientById Recipient { get; set; }
    }
}
