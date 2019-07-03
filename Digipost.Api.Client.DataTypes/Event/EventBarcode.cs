namespace Digipost.Api.Client.DataTypes.Event
{
    public class EventBarcode
    {
        public EventBarcode(string barcodeValue, string barcodeType, string barcodeText, bool showValueInBarcode)
        {
            BarcodeValue = barcodeValue;
            BarcodeType = barcodeType;
            BarcodeText = barcodeText;
            ShowValueInBarcode = showValueInBarcode;
        }
        
        /// <summary>
        ///     The barcode on this receipt.
        /// </summary>
        public string BarcodeValue { get; set; }
        
        /// <summary>
        ///     The type of barcode.
        /// </summary>
        public string BarcodeType { get; set; }
        
        /// <summary>
        ///     Barcode text can be used to describe the barcode.
        /// </summary>
        public string BarcodeText { get; set; }
        
        /// <summary>
        ///     If true, the barcode will render its value as part of the image.
        /// </summary>
        public bool ShowValueInBarcode { get; set; }
        
        internal barcode AsDataTransferObject()
        {
            return new barcode
            {
                barcodeValue = BarcodeValue,
                barcodeType = BarcodeType,
                barcodeText = BarcodeText,
                showValueInBarcode = ShowValueInBarcode
            };
        }

        public override string ToString()
        {
            return $"Barcode: '" +
                   $"{(BarcodeValue != null ? $"{BarcodeValue}, " : "")} " +
                   $"{(BarcodeType != null ? $"{BarcodeType}, " : "")} " +
                   $"{(BarcodeText != null ? $"{BarcodeText}, " : "")}'";
        }
    }
}
