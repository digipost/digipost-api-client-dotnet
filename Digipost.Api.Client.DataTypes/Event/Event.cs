using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Event
{
    public class Event : IDataType
    {
        /// <summary>
        ///     The name of the event.
        ///     Example: Kommunestyre- og fylkestingvalg
        ///     150 characters or less.
        /// </summary>
        public string SubTitle { get; set; }
        
        /// <summary>
        ///     A List of the time intervals of the event.
        /// </summary>
        public List<TimeSpan> Time { get; set; }
        
        /// <summary>
        ///     Optional label for time. null yield default in gui, eg. 'Opening hours'
        /// </summary>
        public string TimeLabel { get; set; }
        
        /// <summary>
        ///     Free text but can contain a ISO8601 DateTime.
        ///     Example: 'Please use entrance from street'
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        ///     The name of the place.
        ///     Example: 'Sagene skole'
        /// </summary>
        public string Place { get; set; }
        
        /// <summary>
        ///     Optional label for place. null yield default in gui, eg. 'Venue location'
        /// </summary>
        public string PlaceLabel { get; set; }
        
        /// <summary>
        ///     Address where the event takes place.
        /// </summary>
        public EventAddress Address { get; set; }
        
        /// <summary>
        ///     Additional sections of information (max 10) with a title and text.
        /// </summary>
        public List<Info> Info { get; set; }
        
        /// <summary>
        ///     Optional label for barcode. null yield default in gui, eg. ''
        /// </summary>
        public string BarcodeLabel { get; set; }
        
        /// <summary>
        ///     The Barcode to be scanned for validation.
        /// </summary>
        public EventBarcode Barcode { get; set; }
        
        /// <summary>
        ///     Links for related information to the event.
        /// </summary>
        public List<ExternalLink> Links { get; set; }
        
        public Event(List<TimeSpan> time)
        {
            Time = time;
        }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal eventAppointment AsDataTransferObject()
        {
            var dto = new eventAppointment
            {
                starttime = StartTime.ToString("O"),
                arrivaltime = ArrivalTime,
                subtitle = SubTitle,
                place = Place,
                endtime = EndTime?.ToString("O"),
                address = AppointmentAddress?.AsDataTransferObject(),
                info = Info?.Select(i => i.AsDataTransferObject()).ToArray()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Event starting at {StartTime}. " +
                   $"End time: '{(EndTime.HasValue ? EndTime.ToString() : "<none>")}', " +
                   $"arrival time: '{ArrivalTime ?? "<none>"}', " +
                   $"{AppointmentAddress?.ToString() ?? "address: <none>"}, " +
                   $"place: '{Place ?? "<none>"}', " +
                   $"sub title: '{SubTitle ?? "<none>"}', " +
                   $"{(Info != null ? string.Join(", ", Info.Select(x => x.ToString())) : "no additional info")}.";
        }
    }
}
