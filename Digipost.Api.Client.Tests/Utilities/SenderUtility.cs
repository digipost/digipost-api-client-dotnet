using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Shared.Certificate;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Utilities
{
    internal class SenderUtility
    {
        public static TestSender GetSender(TestEnvironment testEnvironment)
        {
            switch (testEnvironment)
            {
                case TestEnvironment.DifiTest:
                    return new TestSender(
                        497013,
                        CertificateResource.Certificate(),
                        Environment.DifiTest,
                        new RecipientById(IdentificationType.DigipostAddress, "ReplaceMehere")
                    );
                case TestEnvironment.Qa:
                    return new TestSender(
                        1185201,
                        CertificateReader.ReadCertificate(),
                        Environment.Qa,
                        new RecipientById(IdentificationType.DigipostAddress, "liv.test.aliassen#8514")
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
        public TestSender(long id, X509Certificate2 certificate, Environment environment, DigipostRecipient recipient)
        {
            Id = id;
            Certificate = certificate;
            Environment = environment;
            Recipient = recipient;
        }

        public long Id { get; set; }

        public Environment Environment { get; set; }

        public X509Certificate2 Certificate { get; set; }

        public DigipostRecipient Recipient { get; set; }
    }
}
