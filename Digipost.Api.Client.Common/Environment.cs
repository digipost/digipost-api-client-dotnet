using System;

namespace Digipost.Api.Client
{
    public class Environment
    {
        private Environment(Uri url)
        {
            Url = url;
        }

        public Uri Url { get; set; }

        public static Environment Production => new Environment(new Uri("https://api.digipost.no/"));

        public static Environment NorskHelsenett => new Environment(new Uri("https://api.nhn.digipost.no"));

        [Obsolete("Use Environment.DifiTest instead")]
        public static Environment Preprod => new Environment(new Uri("https://api.difitest.digipost.no/"));

        public static Environment DifiTest => new Environment(new Uri("https://api.difitest.digipost.no/"));

        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}