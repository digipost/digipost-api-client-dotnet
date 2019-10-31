using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class Period : TimePeriode, IDataType
    {
        /// <summary>
        ///     Period start (ISO8601 full DateTime)
        /// </summary>
        public DateTime? From { get; set; }
        
        /// <summary>
        ///     Period end (ISO8601 full DateTime)
        /// </summary>
        public DateTime? To { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal period AsDataTransferObject()
        {
            var dto = new period
            {
                from = From?.ToString("O"),
                to = To?.ToString("O")
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Fra: '{From?.ToString("O") ?? "<none>"}', " +
                   $"Til: '{To?.ToString("O") ?? "<none>"}'";
        }
    }
}
