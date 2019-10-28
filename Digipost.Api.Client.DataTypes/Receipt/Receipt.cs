using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class Receipt : IDataType
    {
        public Receipt(DateTime purchaseTime, decimal totalPrice, decimal totalVat, string merchantName)
        {
            PurchaseTime = purchaseTime;
            TotalPrice = totalPrice;
            TotalVat = totalVat;
            MerchantName = merchantName;
        }

        /// <summary>
        ///     The ID of this receipt in the system it was imported from
        /// </summary>
        public string ReceiptId { get; set; }
        
        /// <summary>
        ///     The original receipt number from the store
        /// </summary>
        public string ReceiptNumber { get; set; }
        
        /// <summary>
        ///     When the purchase was made (ISO8601 full DateTime)
        /// </summary>
        public DateTime PurchaseTime { get; set; }
        
        /// <summary>
        ///     The total price paid for the item(s) purchased
        /// </summary>
        public Decimal TotalPrice { get; set; }
        
        /// <summary>
        ///     The total vat amount for the item(s) purchased
        /// </summary>
        public Decimal TotalVat { get; set; }
        
        /// <summary>
        ///     Currency of the price, ISO4217. Example: NOK
        /// </summary>
        public string CurrencyCode { get; set; }
        
        /// <summary>
        ///     Identifier for cashier who made the sale
        /// </summary>
        public string Cashier { get; set; }
        
        /// <summary>
        ///     Identifier for the register where the purchase was made
        /// </summary>
        public string Register { get; set; }
        
        /// <summary>
        ///     Optional name of the chain that the merchant is a part of
        /// </summary>
        public string MerchantChain { get; set; }
        
        /// <summary>
        ///     Name of the store or merchant. Example: Grünerløkka Hip Coffee
        /// </summary>
        public string MerchantName { get; set; }
        
        /// <summary>
        ///     Merchant phone number
        /// </summary>
        public string MerchantPhoneNumber { get; set; }
        
        /// <summary>
        ///     Address of the store or merchant
        /// </summary>
        public Address MerchantAddress { get; set; }
        
        /// <summary>
        ///     Organization number of the sales point
        /// </summary>
        public string OrganizationNumber { get; set; }
        
        /// <summary>
        ///     Barcode info
        /// </summary>
        public Barcode Barcode { get; set; }
        
        /// <summary>
        ///     List of payments done during this purchase
        /// </summary>
        public List<Payment> Payments { get; set; }
        
        /// <summary>
        ///     The individual items sold
        /// </summary>
        public List<ReceiptLine> Items { get; set; }
        
        /// <summary>
        ///     Details for taxi receipts
        /// </summary>
        public TaxiDetails TaxiDetails { get; set; }
        
        /// <summary>
        ///     Name and address of customer
        /// </summary>
        public Customer Customer { get; set; }
        
        /// <summary>
        ///     Name and address of delivery
        /// </summary>
        public Delivery Delivery { get; set; }
        
        /// <summary>
        ///     Order number
        /// </summary>
        public string OrderNumber { get; set; }
        
        /// <summary>
        ///     Membership number
        /// </summary>
        public string MembershipNumber { get; set; }
        
        /// <summary>
        ///     Comment
        /// </summary>
        public string Comment { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal receipt AsDataTransferObject()
        {
            var dto = new receipt
            {
                receiptId = ReceiptId,
                receiptNumber = ReceiptNumber,
                purchaseTime = PurchaseTime.ToString("O"),
                totalPrice = TotalPrice.ToString("C"),
                totalVat = TotalVat.ToString("C"),
                currency = CurrencyCode,
                cashier = Cashier,
                register = Register,
                merchantchain = MerchantChain,
                merchantname = MerchantName,
                merchantphonenumber = MerchantPhoneNumber,
                merchantaddress = MerchantAddress?.AsDataTransferObject(),
                orgnumber = OrganizationNumber,
                barcode = Barcode?.AsDataTransferObject(),
                payments = Payments?.Select(i => i.AsDataTransferObject()).ToArray(),
                items = Items?.Select(i => i.AsDataTransferObject()).ToArray(),
                taxiDetails = TaxiDetails?.AsDataTransferObject(),
                customer = Customer?.AsDataTransferObject(),
                delivery = Delivery?.AsDataTransferObject(),
                ordernumber = OrderNumber,
                membershipnumber = MembershipNumber,
                comment = Comment
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"ReceiptId: '{ReceiptId}', " +
                   $"ReceiptNumber: '{ReceiptNumber}', " +
                   $"PurchaseTime: '{PurchaseTime}', " +
                   $"TotalPrice: '{TotalPrice}', " +
                   $"TotalVat: '{TotalVat}', " +
                   $"CurrencyCode: '{CurrencyCode}', " +
                   $"Cashier: '{Cashier}', " +
                   $"Register: '{Register}', " +
                   $"MerchantChain: '{MerchantChain}', " +
                   $"MerchantName: '{MerchantName}', " +
                   $"MerchantPhoneNumber: '{MerchantPhoneNumber}', " +
                   $"MerchantAddress: '{MerchantAddress?.ToString() ?? "<none>"}', " +
                   $"OrganizationNumber: '{OrganizationNumber}', " +
                   $"Barcode: '{Barcode?.ToString() ?? "<none>"}', " +
                   $"Payments: '{(Payments != null ? string.Join(", ", Payments.Select(x => x.ToString())) : "no additional info")}', " +
                   $"Items: '{(Items != null ? string.Join(", ", Items.Select(x => x.ToString())) : "no additional info")}', " +
                   $"TaxiDetails: '{TaxiDetails?.ToString() ?? "<none>"}', " +
                   $"Customer: '{Customer?.ToString() ?? "<none>"}', " +
                   $"Delivery: '{Delivery?.ToString() ?? "<none>"}', " +
                   $"OrderNumber: '{OrderNumber}', " +
                   $"MembershipNumber: '{MembershipNumber}', " +
                   $"Comment: '{Comment}'";
        }
    }
}
