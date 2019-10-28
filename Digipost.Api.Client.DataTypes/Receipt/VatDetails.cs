using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class VatDetails : IDataType
    {
        /// <summary>
        ///     Vat Levels
        /// </summary>
        public List<VatLevel> Levels { get; set; }
        
        /// <summary>
        ///     Sum
        /// </summary>
        public Decimal Sum { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal vatDetails AsDataTransferObject()
        {
            var dto = new vatDetails
            {
                levels = Levels?.Select(i => i.AsDataTransferObject()).ToArray(),
                sum = Sum.ToString("C")
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Links: '{(Levels != null ? string.Join(", ", Levels.Select(x => x.ToString())) : "<none>")}', " +
                   $"Sum: '{Sum}'";
        }
    }
}
