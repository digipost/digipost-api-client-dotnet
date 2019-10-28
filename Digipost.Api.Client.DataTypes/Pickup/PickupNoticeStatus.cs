using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class PickupNoticeStatus : IDataType
    {
        public PickupNoticeStatus(Status status)
        {
            Status = status;
        }

        /// <summary>
        ///     The status of the PickupNotice
        /// </summary>
        public Status Status { get; set; }
        
        /// <summary>
        ///     ISO8601 full DateTime for time of occurrence
        /// </summary>
        public DateTime? OccurrenceDateTime { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal pickupNoticeStatus AsDataTransferObject()
        {
            var dto = new pickupNoticeStatus
            {
                status = (status) Enum.Parse(typeof(status), Status.ToString()),
                occurrencedatetime = OccurrenceDateTime?.ToString("O")
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Status: '{Status}', " +
                   $"OccurrenceDateTime: '{OccurrenceDateTime}'";
        }
    }
}
