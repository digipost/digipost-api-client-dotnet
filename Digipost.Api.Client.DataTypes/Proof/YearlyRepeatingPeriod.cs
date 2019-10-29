using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class YearlyRepeatingPeriod : TimePeriode, IDataType
    {
        public YearlyRepeatingPeriod(MonthlyTimePoint from, MonthlyTimePoint to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        ///     Starting year of the repeating period
        /// </summary>
        public int StartYear { get; set; }
        
        /// <summary>
        ///     Ending year of the repeating period
        /// </summary>
        public int EndYear { get; set; }
        
        /// <summary>
        ///     Starting month of the repeating period
        /// </summary>
        public MonthlyTimePoint From { get; set; }
        
        /// <summary>
        ///     Ending month of the repeating period
        /// </summary>
        public MonthlyTimePoint To { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal aarligRepeterendePeriode AsDataTransferObject()
        {
            var dto = new aarligRepeterendePeriode
            {
                startaar = StartYear,
                sluttaar = EndYear,
                fra = From.AsDataTransferObject(),
                til = To.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"StartAar: '{StartYear}', " +
                   $"SluttAar: '{EndYear}', " +
                   $"Fra: '{From}', " +
                   $"Til: '{To}'";
        }
    }
}
