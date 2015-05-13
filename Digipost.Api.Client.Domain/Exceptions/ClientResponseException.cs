using System;

namespace Digipost.Api.Client.Domain.Exceptions
{
    public class ClientResponseException : Exception
    {
        public Error Error { get; set; }

        public ClientResponseException(string message, Error error)
            : base(message)
        {
        }

        public ClientResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ClientResponseException(string message, Error error, Exception inner)
            : base(message, inner)
        {
            Error = error;
        }
    }
}
