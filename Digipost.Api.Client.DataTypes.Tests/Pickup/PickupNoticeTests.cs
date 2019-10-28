using System;
using Digipost.Api.Client.DataTypes.Pickup;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Pickup
{
    public class PickupNoticeTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var time = DateTime.Now;
            var source = new PickupNotice(
                "1234", 
                new Barcode("1234", "type", "text", true), 
                time, 
                time.AddDays(3), 
                new Recipient("Testbert", "testbert#1234"), 
                new PickupPlace("Coop Mega", "1234", "Instructions", new Address("Test Gate", "1234", "Oslo")) 
                );

            var expected = source.AsDataTransferObject();

            var actual = new pickupNotice
            {
                parcelid = "1234",
                barcode = new barcode { barcodetext = "text", barcodetype = "type", barcodevalue = "1234", showvalueinbarcode = true },
                arrivaldatetime = time.ToString("O"),
                returndatetime = time.AddDays(3).ToString("O"),
                recipient = new datatyperecipient { name = "Testbert", digipostaddress = "testbert#1234" },
                pickupplace = new pickupPlace { name = "Coop Mega", code = "1234", instruction = "Instructions", address = new datatypeaddress { streetaddress = "Test Gate", postalcode = "1234", city = "Oslo" } }
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
