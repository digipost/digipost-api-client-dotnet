using System;

namespace Digipost.Api.Client.Common.Exceptions
{
    public class ClientResponseException : Exception
    {
        public ClientResponseException(string message)
            : base(message)
        {
        }

        public ClientResponseException(string message, IError error)
            : base(message)
        {
            Error = error;
        }

        public ClientResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ClientResponseException(string message, IError error, Exception inner)
            : base(message, inner)
        {
            Error = error;
        }

        public IError Error { get; set; }
    }
}