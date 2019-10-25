using System;

namespace Digipost.Api.Client.DataTypes.Event
{
    public class EventTimeSpan
    {
        public EventTimeSpan(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
        
        /// <summary>
        ///     The start time of the span.
        /// </summary>
        public DateTime Start { get; set; }
        
        /// <summary>
        ///     The end time of the span.
        /// </summary>
        public DateTime End { get; set; }

        internal timeInterval AsDataTransferObject()
        {
            return new timeInterval
            {
                starttime = Start.ToString(),
                endtime = End.ToString()
            };
        }

        public override string ToString()
        {
            return $"TimeSpan: '{(Start != null ? $"{Start.ToString()}, " : "")} to {(End != null ? $"{End.ToString()}, " : "")}'";
        }
    }
}
