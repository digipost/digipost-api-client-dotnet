using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class PickupPlace : IDataType
    {
        /// <summary>
        ///     The pickup place name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     The pickup code
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        ///     Instructions for fetching the parcel
        /// </summary>
        public string Instruction { get; set; }
        
        /// <summary>
        ///     Shelf location at pickup point
        /// </summary>
        public string ShelfLocation { get; set; }
        
        /// <summary>
        ///     Address
        /// </summary>
        public Address Address { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal pickupPlace AsDataTransferObject()
        {
            var dto = new pickupPlace
            {
                name = Name,
                code = Code,
                instruction = Instruction,
                shelflocation = ShelfLocation,
                address = Address.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Name: '{Name}', " +
                   $"Code: '{Code}', " +
                   $"Instruction: '{Instruction}', " +
                   $"ShelfLocation: '{ShelfLocation}', " +
                   $"Address: '{Address.ToString()}'";
        }
    }
}
