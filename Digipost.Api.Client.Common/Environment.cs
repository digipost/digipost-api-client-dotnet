using System;

namespace Digipost.Api.Client.Common
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

        public static Environment DifiTest => new Environment(new Uri("https://api.difitest.digipost.no/"));

        public static Environment Test => new Environment(new Uri("https://api.test.digipost.no/"));

        internal static Environment Qa => new Environment(new Uri("https://api.qa.digipost.no/"));

        internal static Environment Local => new Environment(new Uri("http://localhost:8282/"));

        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}
