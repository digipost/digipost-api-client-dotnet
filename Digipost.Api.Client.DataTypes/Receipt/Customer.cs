using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class Customer : IDataType
    {
        /// <summary>
        ///     Customer name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     Customer address
        /// </summary>
        public Address Address{ get; set; }
        
        /// <summary>
        ///     Customer phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal customer AsDataTransferObject()
        {
            var dto = new customer
            {
                name = Name,
                address = Address?.AsDataTransferObject(),
                phoneNumber = PhoneNumber
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Name: '{Name}', " +
                   $"Address: '{Address?.ToString() ?? "<none>"}', " +
                   $"PhoneNumber: '{PhoneNumber}'";
        }
    }
}
