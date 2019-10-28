using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class Delivery : IDataType
    {
        /// <summary>
        ///     Delivery name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     Delivery address
        /// </summary>
        public Address Address { get; set; }
        
        /// <summary>
        ///     Delivery terms
        /// </summary>
        public string Terms { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal delivery AsDataTransferObject()
        {
            var dto = new delivery
            {
                name = Name,
                address = Address?.AsDataTransferObject(),
                terms = Terms
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Name: '{Name}', " +
                   $"Address: '{Address?.ToString() ?? "<none>"}', " +
                   $"Terms: '{Terms}'";
        }
    }
}
