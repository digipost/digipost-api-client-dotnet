using System;
using System.Collections.Generic;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Appointment
{
    public class AppointmentTests
    {
        [Fact]
        public void AllowsOnlyRequiredParameters()
        {
            var now = DateTime.Now;

            var source = new DataTypes.Appointment.Appointment(now);
            var expected = source.AsDataTransferObject();

            var actual = new appointment
            {
                starttime = now.ToString("O")
            };

            Comparator.AssertEqual(expected, actual);
        }

        [Fact]
        public void AsDataTransferObject()
        {
            var now = DateTime.Now;
            var address = new AppointmentAddress("Gateveien 1", "0001", "Oslo");
            var info = new Info("Title", "Very important information");

            var source = new DataTypes.Appointment.Appointment(now)
            {
                EndTime = now.AddHours(1),
                AppointmentAddress = address,
                ArrivalTime = "15 minutes before",
                Info = new List<Info>
                {
                    info
                },
                Place = "Oslo City Røntgen",
                SubTitle = "SubTitle"
            };
            var expected = source.AsDataTransferObject();

            var actual = new appointment
            {
                starttime = now.ToString("O"),
                endtime = now.AddHours(1).ToString("O"),
                arrivaltime = "15 minutes before",
                address = address.AsDataTransferObject(),
                info = new[]
                {
                    info.AsDataTransferObject()
                },
                place = "Oslo City Røntgen",
                subtitle = "SubTitle"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
