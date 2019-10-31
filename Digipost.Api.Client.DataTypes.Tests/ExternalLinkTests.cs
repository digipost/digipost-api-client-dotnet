using System;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests
{
    public class ExternalLinkTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var now = DateTime.Now;
            var source = new ExternalLink(new Uri("https://digipost.no"))
            {
                ButtonText = "Click me",
                Deadline = now,
                Description = "Description"
            };
            var expected = source.AsDataTransferObject();

            var actual = new externalLink
            {
                buttontext = "Click me",
                deadline = now,
                description = "Description",
                url = "https://digipost.no/"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
