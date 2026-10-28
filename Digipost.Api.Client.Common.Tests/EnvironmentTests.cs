using System;
using Xunit;

namespace Digipost.Api.Client.Common.Tests
{
    public class EnvironmentTests
    {
        [Fact]
        public void Can_Change_Url()
        {
            var env = Environment.DifiTest;
            env.Url = new Uri("http://api.newenvironment.digipost.no");
        }

        [Fact]
        public void Can_Change_TokenEndpoint()
        {
            var env = Environment.DifiTest;
            env.TokenEndpoint = new Uri("http://midp.newenvironment.digipost.no/oauth2/token");
        }
    }
}
