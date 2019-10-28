using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests
{
    public class InfoTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var source = new Info("Title", "Text");
            var expected = source.AsDataTransferObject();

            var actual = new info
            {
                title = "Title",
                text = "Text"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
