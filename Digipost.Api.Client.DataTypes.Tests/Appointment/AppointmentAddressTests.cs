using Digipost.Api.Client.DataTypes.Appointment;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Appointment
{
    public class AppointmentAddressTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var source = new Address("Gateveien 1", "0001", "Oslo");
            var expected = source.AsDataTransferObject();

            var actual = new datatypeaddress
            {
                streetaddress = "Gateveien 1",
                postalcode = "0001",
                city = "Oslo"
            };

            Comparator.AssertEqual(expected, actual);
        }

        [Fact]
        public void AsDataTransferObjectWithoutStreetAddress()
        {
            var expected = new Address("0001", "Oslo");

            var actual = new datatypeaddress
            {
                streetaddress = null,
                postalcode = "0001",
                city = "Oslo"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
