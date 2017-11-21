using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Appointment
{
    public class AppointmentTests
    {
        private static readonly Comparator Comparator = new Comparator();

        [Fact]
        public void AsDataTransferObject()
        {
            var now = DateTime.Parse("2017-11-21T13:00:00+01:00");
            var address = new AppointmentAddress("Gateveien 1", "0001", "Oslo");
            var info = new Info("Title", "Very important information");

            var source = new DataTypes.Appointment.Appointment(now)
            {
                EndTime = now.AddHours(1),
                AppointmentAddress = address,
                ArrivalTime = "15 minutes before",
                Info = new List<Info>()
                {
                    info
                },
                Place = "Oslo City Røntgen",
                SubTitle = "SubTitle"
            };
            var expected = source.AsDataTransferObject();

            var actual = new appointment()
            {
                starttime = "2017-11-21T13:00:00.0000000+01:00",
                endtime = "2017-11-21T14:00:00.0000000+01:00",
                arrivaltime = "15 minutes before",
                address = address.AsDataTransferObject(),
                info = new info[]
                {
                    info.AsDataTransferObject()
                },
                place = "Oslo City Røntgen",
                subtitle = "SubTitle"
            };

            IEnumerable<IDifference> differences;
            Comparator.Equal(expected, actual, out differences);
            Assert.Empty(differences);
        }

        [Fact]
        public void AllowsOnlyRequiredParameters()
        {
            var now = DateTime.Parse("2017-11-21T13:00:00+01:00");

            var source = new DataTypes.Appointment.Appointment(now);
            var expected = source.AsDataTransferObject();

            var actual = new appointment()
            {
                starttime = "2017-11-21T13:00:00.0000000+01:00",
            };

            IEnumerable<IDifference> differences;
            Comparator.Equal(expected, actual, out differences);
            Assert.Empty(differences);
        }

    }
}
