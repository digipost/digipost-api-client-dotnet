using System;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Receipt
{
    public class ReceiptTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var time = DateTime.Now;
            
            var source = new DataTypes.Receipt.Receipt(time, 1500m, 24m, "Testbert");

            var expected = source.AsDataTransferObject();

            var actual = new receipt
            {
                purchaseTime = time.ToString("O"),
                totalPrice = 1500m.ToString("C"),
                totalVat = 24m.ToString("C"),
                merchantname = "Testbert"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
