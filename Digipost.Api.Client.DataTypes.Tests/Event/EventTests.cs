using System;
using System.Collections.Generic;
using Digipost.Api.Client.DataTypes.Event;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Event
{
    public class EventTests
    {
        [Fact]
        public void AllowsOnlyRequiredParameters()
        {
            var ets = new List<EventTimeSpan>();
            var ts = new EventTimeSpan(DateTime.Today, DateTime.Today.AddHours(3));
            ets.Add(ts);

            var source = new DataTypes.Event.Event(ets);
            var expected = source.AsDataTransferObject();

            var nets = new timeInterval[1];
            nets[0] = new timeInterval {starttime = DateTime.Today.ToString(), endtime = DateTime.Today.AddHours(3).ToString()};

            var actual = new @event
            {
                starttime = nets
            };

            Comparator.AssertEqual(expected, actual);
        }

        [Fact]
        public void AsDataTransferObject()
        {
            var ets = new List<EventTimeSpan>();
            var ts = new EventTimeSpan(DateTime.Today, DateTime.Today.AddHours(3));
            ets.Add(ts);
            var barcode = new Barcode("12345678", "insert type here", "this is a code", true);
            var address = new Address("Gateveien 1", "0001", "Oslo");
            var info = new Info("Title", "Very important information");
            var links = new List<ExternalLink>();

            var source = new DataTypes.Event.Event(ets)
            {
                Description = "Description here",
                Address = address,
                Info = new List<Info>
                {
                    info
                },
                Place = "Oslo City Røntgen",
                PlaceLabel = "This is a place",
                SubTitle = "SubTitle",
                Barcode = barcode,
                BarcodeLabel = "Barcode Label",
                Links = links
            };
            var expected = source.AsDataTransferObject();

            var nets = new timeInterval[1];
            nets[0] = new timeInterval {starttime = DateTime.Today.ToString(), endtime = DateTime.Today.AddHours(3).ToString()};
            var nlinks = new externalLink[0];
            
            var actual = new @event
            {
                starttime = nets,
                description = "Description here",
                address = address.AsDataTransferObject(),
                info = new[]
                {
                    info.AsDataTransferObject()
                },
                place = "Oslo City Røntgen",
                placeLabel = "This is a place",
                subtitle = "SubTitle",
                barcode = barcode.AsDataTransferObject(),
                barcodeLabel = "Barcode Label",
                links = nlinks
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
