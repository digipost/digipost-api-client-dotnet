using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class MonthlyTimePoint : IDataType
    {
        public MonthlyTimePoint(int month, int day)
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
        ///     Hour (1-23)
        /// </summary>
        public int Hour { get; set; }
        
        /// <summary>
        ///     Minute (1-59)
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
        
        internal maanedsTidspunkt AsDataTransferObject()
        {
            var dto = new maanedsTidspunkt
            {
                maaned = Month,
                dag = Day,
                time = Hour,
                min = Min,
                tidssone = TimeZone
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Maaned: '{Month}', " +
                   $"Dag: '{Day}', " +
                   $"Time: '{Hour}', " +
                   $"Min: '{Min}', " +
                   $"Tidssone: '{TimeZone}'";
        }
    }
}
