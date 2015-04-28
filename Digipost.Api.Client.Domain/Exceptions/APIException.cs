using System;

namespace Digipost.Api.Client.Domain.Exceptions
{
    public class APIException : Exception
    {
        public APIException(string message) : base(message)
        {
            
        }

        public APIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
