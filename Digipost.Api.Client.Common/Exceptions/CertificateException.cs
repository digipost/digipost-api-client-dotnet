using System;

namespace Digipost.Api.Client.Common.Exceptions
{
    public class CertificateException : Exception
    {
        public CertificateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
