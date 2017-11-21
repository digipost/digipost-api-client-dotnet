using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Appointment
{
    public class Appointment : IDataType
    {

        public DateTime StartTime { get; set; }

        /// <summary>
        /// Default value 30 minutes after <see cref="StartTime"/>.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Free text but can contain ISO8601 formatted date and time. Example: Please arrive 15 minutes early.
        /// </summary>
        public String ArrivalTime { get; set; }

        /// <summary>
        /// The name of the place. Example: Oslo City Røntgen
        /// </summary>
        public String Place { get; set; }

        public AppointmentAddress AppointmentAddress { get; set; }

        /// <summary>
        /// Example: MR-undersøkelse av høyre kne
        /// </summary>
        public String SubTitle { get; set; }

        /// <summary>w
        /// Additional sections of information (max 2) with a title and text
        /// </summary>
        public List<Info> Info { get; set; }

        public Appointment(DateTime startTime)
        {
            StartTime = startTime;
        }

        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }

        protected appointment AsDataTransferObject()
        {
            var dto = new appointment()
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

    }
}
