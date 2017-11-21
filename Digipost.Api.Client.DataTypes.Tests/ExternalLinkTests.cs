using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests
{
    public class ExternalLinkTests
    {
        private static readonly Comparator Comparator = new Comparator();

        [Fact]
        public void AsDataTransferObject()
        {
            var now = DateTime.Now;
            var expected = new ExternalLink(new Uri("https://digipost.no"))
            {
                ButtonText = "Click me",
                Deadline = now,
                Description = "Description"
            };

            var actual = new externalLink()
            {
                buttontext = "Click me",
                deadline = now,
                deadlineSpecified = true,
                description = "Description",
                url = "https://digipost.no"
            };

            IEnumerable<IDifference> differences;
            Comparator.Equal(expected, actual, out differences);
            Assert.Empty(differences);
        }
    }
}
