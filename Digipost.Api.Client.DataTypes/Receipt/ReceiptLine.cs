using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class ReceiptLine : IDataType
    {
        /// <summary>
        ///     Item name
        /// </summary>
        public string ItemName { get; set; }
        
        /// <summary>
        ///     Item description
        /// </summary>
        public string ItemDescription { get; set; }
        
        /// <summary>
        ///     Item code
        /// </summary>
        public string ItemCode { get; set; }
        
        /// <summary>
        ///     The unit that the item is measured in
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        ///     Quantity
        /// </summary>
        public double Quantity { get; set; }
        
        /// <summary>
        ///     Item price
        /// </summary>
        public Decimal ItemPrice { get; set; }
        
        /// <summary>
        ///     Item VAT
        /// </summary>
        public Decimal ItemVat { get; set; }
        
        /// <summary>
        ///     Total price
        /// </summary>
        public Decimal TotalPrice { get; set; }
        
        /// <summary>
        ///     Total VAT
        /// </summary>
        public Decimal TotalVat { get; set; }
        
        /// <summary>
        ///     Discout
        /// </summary>
        public Decimal Discount { get; set; }
        
        /// <summary>
        ///     Serial Number
        /// </summary>
        public string SerialNumber { get; set; }
        
        /// <summary>
        ///     EAN Code
        /// </summary>
        public string EanCode { get; set; }

        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal receiptLine AsDataTransferObject()
        {
            var dto = new receiptLine
            {
                itemname = ItemName,
                itemdescription = ItemDescription,
                itemcode = ItemCode,
                unit = Unit,
                quantity = Quantity,
                itemprice = ItemPrice.ToString("C"),
                itemvat = ItemVat.ToString("C"),
                totalprice = TotalPrice.ToString("C"),
                totalvat = TotalVat.ToString("C"),
                discount = Discount.ToString("C"),
                serialNumber = SerialNumber,
                eanCode = EanCode
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"ItemName: '{ItemName}', " +
                   $"ItemDescription: '{ItemDescription}', " +
                   $"ItemCode: '{ItemCode}', " +
                   $"Unit: '{Unit}', " +
                   $"Quantity: '{Quantity}', " +
                   $"ItemPrice: '{ItemPrice}', " +
                   $"ItemVat: '{ItemVat}', " +
                   $"TotalPrice: '{TotalPrice}', " +
                   $"TotalVat: '{TotalVat}', " +
                   $"Discount: '{Discount}', " +
                   $"SerialNumber: '{SerialNumber}', " +
                   $"EanCode: '{EanCode}'";
        }
    }
}
