using System.IO;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Tests.Fakes
{
    internal class FakeDocument : Document
    {
        public FakeDocument(string subject, string fileType, byte[] contentBytes, AuthenticationLevel authenticationLevel = AuthenticationLevel.Password, SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : base(subject, fileType, contentBytes, authenticationLevel, sensitivityLevel, smsNotification)
        {
        }

        public FakeDocument(string subject, string fileType, Stream documentStream, AuthenticationLevel authenticationLevel = AuthenticationLevel.Password, SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : base(subject, fileType, documentStream, authenticationLevel, sensitivityLevel, smsNotification)
        {
        }

        public FakeDocument(string subject, string fileType, string path, AuthenticationLevel authenticationLevel = AuthenticationLevel.Password, SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : base(subject, fileType, path, authenticationLevel, sensitivityLevel, smsNotification)
        {
        }

        internal override byte[] ReadAllBytes(Stream documentStream)
        {
            return new byte[] {1, 2, 3};
        }

        internal override byte[] ReadAllBytes(string pathToDocument)
        {
            return new byte[] {1, 2, 3, 4};
        }
    }
}