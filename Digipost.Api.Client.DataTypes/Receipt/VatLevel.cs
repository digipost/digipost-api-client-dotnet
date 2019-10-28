using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class VatLevel : IDataType
    {
        /// <summary>
        ///     Gross amount
        /// </summary>
        public Decimal GrossAmount { get; set; }
        
        /// <summary>
        ///     Net amount
        /// </summary>
        public Decimal NetAmount { get; set; }
        
        /// <summary>
        ///     VAT
        /// </summary>
        public Decimal Vat { get; set; }
        
        /// <summary>
        ///     VAT Percent
        /// </summary>
        public Decimal VatPercent { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal vatLevel AsDataTransferObject()
        {
            var dto = new vatLevel
            {
                grossAmount = GrossAmount.ToString("C"),
                netAmount = NetAmount.ToString("C"),
                vat = Vat.ToString("C"),
                vatPercent = VatPercent.ToString("C")
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"GrossAmount: '{GrossAmount}', " +
                   $"NetAmount: '{NetAmount}', " +
                   $"Vat: '{Vat}', " +
                   $"VatPercent: '{VatPercent}'";
        }
    }
}
