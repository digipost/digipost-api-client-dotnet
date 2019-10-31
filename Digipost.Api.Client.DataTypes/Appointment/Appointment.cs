using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Appointment
{
    public class Appointment : IDataType
    {
        public Appointment(DateTime startTime)
        {
            StartTime = startTime;
        }

        public DateTime StartTime { get; set; }

        /// <summary>
        ///     Default value 30 minutes after <see cref="StartTime" />.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        ///     Free text but can contain ISO8601 formatted date and time. Example: Please arrive 15 minutes early.
        ///     150 characters or less.
        /// </summary>
        public string ArrivalTime { get; set; }

        /// <summary>
        ///     The name of the place. Example: Oslo City Røntgen.
        ///     150 characters or less.
        /// </summary>
        public string Place { get; set; }

        public Address AppointmentAddress { get; set; }

        /// <summary>
        ///     Example: MR-undersøkelse av høyre kne.
        ///     150 characters or less.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        ///     Additional sections of information (max 2) with a title and text.
        /// </summary>
        public List<Info> Info { get; set; }

        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }

        internal appointment AsDataTransferObject()
        {
            var dto = new appointment
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
            return $"Appointment starting at {StartTime}. " +
                   $"End time: '{(EndTime.HasValue ? EndTime.ToString() : "<none>")}', " +
                   $"Arrival time: '{ArrivalTime ?? "<none>"}', " +
                   $"{AppointmentAddress?.ToString() ?? "address: <none>"}, " +
                   $"Place: '{Place ?? "<none>"}', " +
                   $"Subtitle: '{SubTitle ?? "<none>"}', " +
                   $"{(Info != null ? string.Join(", ", Info.Select(x => x.ToString())) : "no additional info")}.";
        }
    }
}
