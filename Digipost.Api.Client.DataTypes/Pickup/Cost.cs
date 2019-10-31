using System;
using System.Numerics;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class Cost : IDataType
    {
        public Cost(decimal valueToBePayed)
        {
            ValueToBePayed = valueToBePayed;
        }

        /// <summary>
        ///     The value of the parcel in NOK
        /// </summary>
        public decimal ValueToBePayed { get; set; }
        
        /// <summary>
        ///     The value of the parcel in NOK
        /// </summary>
        public decimal? PackageValue { get; set; }
        
        /// <summary>
        ///     Paid fee in customs
        /// </summary>
        public decimal? CustomsFeeOutlaid { get; set; }
        
        /// <summary>
        ///     Information about the value added service (vas)
        /// </summary>
        public string VasText { get; set; }
        
        /// <summary>
        ///     Fee paid for customs declaration
        /// </summary>
        public decimal? CustomsFee { get; set; }
        
        /// <summary>
        ///     Outlay for customs by the service
        /// </summary>
        public decimal? CustomsFeeOutlayCost { get; set; }
        
        /// <summary>
        ///     Cash on delivery (cod) amount
        /// </summary>
        public decimal? CodAmount { get; set; }
        
        /// <summary>
        ///     Cash on delivery (cod) fee
        /// </summary>
        public decimal? CodFee { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal cost AsDataTransferObject()
        {
            var dto = new cost
            {
                valuetobepayed = ValueToBePayed,
                packagevalue = PackageValue.GetValueOrDefault(0),
                packagevalueSpecified = PackageValue.HasValue,
                customsfeeoutlayed = CustomsFeeOutlayCost.GetValueOrDefault(0),
                customsfeeoutlayedSpecified = CustomsFeeOutlayCost.HasValue,
                vastext = VasText,
                customsfee = CustomsFee.GetValueOrDefault(0),
                customsfeeSpecified = CustomsFee.HasValue,
                customsfeeoutlaycost = CustomsFeeOutlayCost.GetValueOrDefault(0),
                customsfeeoutlaycostSpecified = CustomsFeeOutlayCost.HasValue,
                codamount = CodAmount.GetValueOrDefault(0),
                codamountSpecified = CodAmount.HasValue,
                codfee = CodFee.GetValueOrDefault(0),
                codfeeSpecified = CodFee.HasValue
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"ValueToBePayed: '{ValueToBePayed}', " +
                   $"PackageValue: '{PackageValue}', " +
                   $"CustomsFeeOutlaid: '{CustomsFeeOutlaid}', " +
                   $"VasText: '{VasText}', " +
                   $"CustomsFee: '{CustomsFee}', " +
                   $"CustomsFeeOutlayCost: '{CustomsFeeOutlayCost}', " +
                   $"CodAmount: '{CodAmount}', " +
                   $"CodFee: '{CodFee}'";
        }
    }
}
