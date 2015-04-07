using System;

namespace Digipost.Api.Client.Domain.Exceptions
{
    public class ConfigException : Exception
    {
        public ConfigException(string message) : base(message)
        {
            
        }

        public ConfigException(string message, Exception inner) : base (message, inner)
        {
            
        }
    }
}
