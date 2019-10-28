using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class Sender : IDataType
    {
        /// <summary>
        ///     The senders name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     The sender's reference
        /// </summary>
        public string Reference { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal sender AsDataTransferObject()
        {
            var dto = new sender
            {
                name = Name,
                reference = Reference
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Name: '{Name}', " +
                   $"Reference: '{Reference}'";
        }
    }
}
