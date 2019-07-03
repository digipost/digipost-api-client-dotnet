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
            var source = new AppointmentAddress("Gateveien 1", "0001", "Oslo");
            var expected = source.AsDataTransferObject();

            var actual = new appointmentAddress
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
            var expected = new AppointmentAddress("0001", "Oslo");

            var actual = new appointmentAddress
            {
                streetaddress = null,
                postalcode = "0001",
                city = "Oslo"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
