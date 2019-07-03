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
            var source = new EventBarcode("12345678", "BarcodeType", "This is a barcode", true);
            var expected = source.AsDataTransferObject();

            var actual = new barcode()
            {
                barcodeValue = "12345678",
                barcodeType= "BarcodeType",
                barcodeText = "This is a barcode",
                showValueInBarcode = true
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
