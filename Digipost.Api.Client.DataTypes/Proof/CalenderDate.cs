using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class CalenderDate : IDataType
    {
        public CalenderDate(int month, int day)
        {
            Month = month;
            Day = day;
        }

        /// <summary>
        ///     Month (1-12)
        /// </summary>
        public int Month { get; set; }
        
        /// <summary>
        ///     Day (1-31)
        /// </summary>
        public int Day { get; set; }
        
        /// <summary>
        ///     Hour (0-23)
        /// </summary>
        public int Hour { get; set; }
        
        /// <summary>
        ///     Minute (0-59)
        /// </summary>
        public int Min { get; set; }
        
        /// <summary>
        ///     Time zone based on the ISO8601 standard
        /// </summary>
        public string TimeZone { get; set; }

        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal calendarDate AsDataTransferObject()
        {
            var dto = new calendarDate
            {
                month = Month,
                day = Day,
                hour = Hour,
                min = Min,
                timezone = TimeZone
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Month: '{Month}', " +
                   $"Day: '{Day}', " +
                   $"Hour: '{Hour}', " +
                   $"Min: '{Min}', " +
                   $"Time Zone: '{TimeZone}'";
        }
    }
}
