using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class Periode : TidsPeriode, IDataType
    {
        /// <summary>
        ///     Periode start (ISO8601 full DateTime)
        /// </summary>
        public DateTime? Fra { get; set; }
        
        /// <summary>
        ///     Periode end (ISO8601 full DateTime)
        /// </summary>
        public DateTime? Til { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal periode AsDataTransferObject()
        {
            var dto = new periode
            {
                fra = Fra?.ToString("O"),
                til = Til?.ToString("O")
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Fra: '{Fra?.ToString("O") ?? "<none>"}', " +
                   $"Til: '{Til?.ToString("O") ?? "<none>"}'";
        }
    }
}
