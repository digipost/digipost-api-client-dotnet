using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class Package : IDataType
    {
        /// <summary>
        ///     Package length in cm
        /// </summary>
        public int Length { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal cost AsDataTransferObject()
        {
            var dto = new cost
            {
                valuetobepayed = ValueToBePayed,
                packagevalue = PackageValue,
                customsfeeoutlayed = CustomsFeeOutlaid,
                vastext = VasText,
                customsfee = CustomsFee,
                customsfeeoutlaycost = CustomsFeeOutlayCost,
                codamount = CodAmount,
                codfee = CodFee
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
