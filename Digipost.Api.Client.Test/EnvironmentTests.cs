using System;
using Xunit;

namespace Digipost.Api.Client.Test
{
    public class EnvironmentTests
    {
        [Fact]
        public void Can_Change_Url()
        {
            var env = Environment.Preprod;
            env.Url = new Uri("http://api.newenvironment.digipost.no");
        }
    }
}