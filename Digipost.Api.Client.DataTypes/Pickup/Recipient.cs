using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class Recipient : IDataType
    {
        /// <summary>
        ///     The name of the recipient
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     The digipost address for the recipient
        /// </summary>
        public string DigipostAddress { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal datatyperecipient AsDataTransferObject()
        {
            var dto = new datatyperecipient
            {
                name = Name,
                digipostaddress = DigipostAddress
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Name: '{Name}', " +
                   $"DigipostAddress: '{DigipostAddress}'";
        }
    }
}
