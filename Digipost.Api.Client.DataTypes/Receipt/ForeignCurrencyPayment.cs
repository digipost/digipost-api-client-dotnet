using System;
using System.Numerics;
using System.Xml;

namespace Digipost.Api.Client.DataTypes
{
    public class ForeignCurrencyPayment : IDataType
    {
        /// <summary>
        ///     Currency of the payment, ISO4217. Example: NOK
        /// </summary>
        public string CurrencyCode { get; set; }
        
        /// <summary>
        ///     Amount
        /// </summary>
        public Decimal? Amount { get; set; }
        
        /// <summary>
        ///     Exchange rate
        /// </summary>
        public Decimal? ExchangeRate { get; set; }
        
        /// <summary>
        ///     Label
        /// </summary>
        public string Label { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal foreignCurrencyPayment AsDataTransferObject()
        {
            var dto = new foreignCurrencyPayment
            {
                currencycode = CurrencyCode,
                amount = Amount,
                exchangerate = ExchangeRate,
                label = Label
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"CurrencyCode: '{CurrencyCode}', " +
                   $"Amount: '{Amount}', " +
                   $"ExchangeRate: '{ExchangeRate}', " +
                   $"Label: '{Label}'";
        }
    }
}
