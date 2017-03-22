using System;

namespace Digipost.Api.Client.Common.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {
        }

        public ApiException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}