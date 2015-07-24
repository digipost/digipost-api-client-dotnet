using System;

namespace Digipost.Api.Client.Domain.Exceptions
{
    public class ClientResponseException : Exception
    {
        public ClientResponseException(string message)
            : base(message)
        {
        }

        public ClientResponseException(string message, Error error)
            : base(message)
        {
            Error = error;
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

        public Error Error { get; set; }
    }
}