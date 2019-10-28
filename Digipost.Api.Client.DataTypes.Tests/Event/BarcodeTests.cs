using Digipost.Api.Client.DataTypes.Event;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Event
{
    public class BarcodeTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var source = new Barcode("12345678", "BarcodeType", "This is a barcode", true);
            var expected = source.AsDataTransferObject();

            var actual = new barcode
            {
                barcodevalue = "12345678",
                barcodetype= "BarcodeType",
                barcodetext = "This is a barcode",
                showvalueinbarcode = true
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
