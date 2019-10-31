using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class YearlyRepeatingPeriod : TimePeriode, IDataType
    {
        public YearlyRepeatingPeriod(CalenderDate from, CalenderDate to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        ///     Starting year of the repeating period
        /// </summary>
        public int? StartYear { get; set; }
        
        /// <summary>
        ///     Ending year of the repeating period
        /// </summary>
        public int? EndYear { get; set; }
        
        /// <summary>
        ///     Starting month of the repeating period
        /// </summary>
        public CalenderDate From { get; set; }
        
        /// <summary>
        ///     Ending month of the repeating period
        /// </summary>
        public CalenderDate To { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal yearlyRepeatingPeriod AsDataTransferObject()
        {
            var dto = new yearlyRepeatingPeriod
            {
                startyear = StartYear.GetValueOrDefault(0),
                startyearSpecified = StartYear.HasValue,
                endyear = EndYear.GetValueOrDefault(0),
                endyearSpecified = EndYear.HasValue,
                from = From.AsDataTransferObject(),
                to = To.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Start Year: '{StartYear}', " +
                   $"End Year: '{EndYear}', " +
                   $"From: '{From}', " +
                   $"To: '{To}'";
        }
    }
}
