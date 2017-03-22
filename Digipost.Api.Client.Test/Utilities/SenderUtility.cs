using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Test.Utilities
{
    internal class SenderUtility
    {
        public static Sender GetSender(TestEnvironment testEnvironment)
        {
            var digipostTestintegrasjonforDigitalPostThumbprint = "‎2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed";

            switch (testEnvironment)
            {
                case TestEnvironment.DifiTest:
                    return new Sender(
                        "497013",
                        digipostTestintegrasjonforDigitalPostThumbprint,
                        Environment.DifiTest,
                        new RecipientById(IdentificationType.DigipostAddress, "ReplaceMehere")
                    );
                case TestEnvironment.Qa:
                    return new Sender(
                        "1010",
                        digipostTestintegrasjonforDigitalPostThumbprint,
                        Environment.Qa,
                        new RecipientById(IdentificationType.DigipostAddress, "digipost.testintegrasjon.for.digita#VTGM")
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

    internal class Sender
    {
        public Sender(string id, string certificateThumbprint, Environment environment, RecipientById recipientById)
        {
            Id = id;
            Certificate = CertificateUtility.SenderCertificate(certificateThumbprint, Language.English);
            Environment = environment;
            Recipient = recipientById;
        }

        public string Id { get; set; }

        public Environment Environment { get; set; }

        public X509Certificate2 Certificate { get; set; }

        public RecipientById Recipient { get; set; }
    }
}