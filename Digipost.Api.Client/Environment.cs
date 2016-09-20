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

        public static Environment Test => new Environment(new Uri("https://api.test.digipost.no/"));

        public static Environment Qa => new Environment(new Uri("https://qa.api.digipost.no/"));

        public static Environment Preprod => new Environment(new Uri("https://qaoffentlig.api.digipost.no/"));

        
        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}