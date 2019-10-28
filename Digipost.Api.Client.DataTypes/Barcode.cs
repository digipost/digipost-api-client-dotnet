using System.Xml;

namespace Digipost.Api.Client.DataTypes
{
    public class Barcode : IDataType
    {
        public Barcode(string barcodeValue, string barcodeType, string barcodeText, bool showValueInBarcode)
        {
            BarcodeValue = barcodeValue;
            BarcodeType = barcodeType;
            BarcodeText = barcodeText;
            ShowValueInBarcode = showValueInBarcode;
        }
        
        /// <summary>
        ///     The barcode on this receipt
        /// </summary>
        public string BarcodeValue { get; set; }
        
        /// <summary>
        /// </summary>
        public string BarcodeType { get; set; }
        
        /// <summary>
        ///     Barcode text can be used to describe the barcode
        /// </summary>
        public string BarcodeText { get; set; }
        
        /// <summary>
        ///     If true, the barcode will render its value as part of the image
        /// </summary>
        public bool ShowValueInBarcode { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal barcode AsDataTransferObject()
        {
            var dto = new barcode
            {
                barcodevalue = BarcodeValue,
                barcodetype = BarcodeType,
                barcodetext = BarcodeText,
                showvalueinbarcode = ShowValueInBarcode
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"BarcodeValue: '{BarcodeValue}', " +
                   $"BarcodeType: '{BarcodeType}', " +
                   $"BarcodeText: '{BarcodeText}', " +
                   $"ShowValueInBarcode: '{ShowValueInBarcode}'";
        }
    }
}
