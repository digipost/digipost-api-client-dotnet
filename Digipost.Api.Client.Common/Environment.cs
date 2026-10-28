using System;

namespace Digipost.Api.Client.Common
{
    public class Environment
    {
        private Environment(Uri url, Uri tokenEndpoint)
        {
            Url = url;
            TokenEndpoint = tokenEndpoint;
        }

        public Uri Url { get; set; }

        public Uri TokenEndpoint { get; set; }

        public static Environment Production => new Environment(new Uri("https://api.digipost.no/"), new Uri("https://midp.digipost.no/oauth2/token"));

        public static Environment NorskHelsenett => new Environment(new Uri("https://api.nhn.digipost.no"), new Uri("https://midp.nhn.digipost.no/oauth2/token"));

        public static Environment DifiTest => new Environment(new Uri("https://api.difitest.digipost.no/"), new Uri("https://midp.difitest.digipost.no/oauth2/token"));

        public static Environment Test => new Environment(new Uri("https://api.test.digipost.no/"), new Uri("https://midp.test.digipost.no/oauth2/token"));

        internal static Environment Qa => new Environment(new Uri("https://api.qa.digipost.no/"), new Uri("https://midp.qa.digipost.no/oauth2/token"));

        internal static Environment Local => new Environment(new Uri("http://localhost:8282/"), new Uri("https://localhost:8043/oauth2/token"));

        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}
