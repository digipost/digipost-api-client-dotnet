using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class Payment : IDataType
    {
        /// <summary>
        ///     Payment type. Examples: Credit Card, BankAxept, Cash
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        ///     The obscured card number associated with the purchase
        /// </summary>
        public string CardNumber { get; set; }
        
        /// <summary>
        ///     The card name
        /// </summary>
        public string CardName { get; set; }
        
        /// <summary>
        ///     Amount paid in this payment
        /// </summary>
        public string Amount { get; set; }
        
        /// <summary>
        ///     Currency of the payment, ISO4217. Example: NOK"
        /// </summary>
        public string CurrencyCode { get; set; }
        
        /// <summary>
        ///     The foreign currency payment info
        /// </summary>
        public ForeignCurrencyPayment ForeignCurrencyPayment { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal payment AsDataTransferObject()
        {
            var dto = new payment
            {
                type = Type,
                cardnumber = CardNumber,
                cardName = CardName,
                amount = Amount,
                currencycode = CurrencyCode,
                foreigncurrencypayment = ForeignCurrencyPayment?.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Type: '{Type}', " +
                   $"CardNumber: '{CardNumber}', " +
                   $"CardName: '{CardName}', " +
                   $"Amount: '{Amount}', " +
                   $"ForeignCurrencyPayment: '{ForeignCurrencyPayment?.ToString() ?? "<none>"}'";
        }
    }
}
