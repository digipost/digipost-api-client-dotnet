using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class Package : IDataType
    {
        /// <summary>
        ///     Package length in cm
        /// </summary>
        public int? Length { get; set; }
        
        /// <summary>
        ///     Package width in cm
        /// </summary>
        public int? Width { get; set; }
        
        /// <summary>
        ///     Package height in cm
        /// </summary>
        public int? Height { get; set; }
        
        /// <summary>
        ///     Package weight in cm
        /// </summary>
        public int? Weight { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal package AsDataTransferObject()
        {
            var dto = new package
            {
                length = Length.GetValueOrDefault(0),
                lengthSpecified = Length.HasValue,
                width = Width.GetValueOrDefault(0),
                widthSpecified = Width.HasValue,
                height = Height.GetValueOrDefault(0),
                heightSpecified = Height.HasValue,
                weight = Weight.GetValueOrDefault(0),
                weightSpecified = Weight.HasValue
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Length: '{Length}', " +
                   $"Width: '{Width}', " +
                   $"Height: '{Height}', " +
                   $"Weight: '{Weight}'";
        }
    }
}
