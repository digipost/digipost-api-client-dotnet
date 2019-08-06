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
            //var digipostTestintegrasjonforDigitalPostThumbprint = "‎2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed";

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
                        CertificateResource.PostenCertificate(),
                        Environment.Qa,
                        //new RecipientById(IdentificationType.DigipostAddress, "digipost.testintegrasjon.for.digita#VZJS")
                        new RecipientByNameAndAddress("Jarand-Bjarte Tysseng Kvistdahl Grindheim", "Digipost Testgate 2A", "0467", "Oslo")
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
