using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class Recipient : IDataType
    {
        public Recipient(string name, string digipostAddress)
        {
            Name = name;
            DigipostAddress = digipostAddress;
        }

        /// <summary>
        ///     The name of the recipient
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     The digipost address for the recipient
        /// </summary>
        public string DigipostAddress { get; set; }
        
        /// <summary>
        ///     The sender's address
        /// </summary>
        public Address Address { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal datatyperecipient AsDataTransferObject()
        {
            var dto = new datatyperecipient
            {
                name = Name,
                digipostaddress = DigipostAddress,
                address = Address.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Name: '{Name}', " +
                   $"DigipostAddress: '{DigipostAddress}', " + 
                   $"Address: '{Address}'";
        }
    }
}
