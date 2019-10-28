using System.Xml;

namespace Digipost.Api.Client.DataTypes.Boligdetaljer
{
    public class Hjemmelshaver : IDataType
    {
        /// <summary>
        ///     
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     
        /// </summary>
        public string Email { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal hjemmelshaver AsDataTransferObject()
        {
            var dto = new hjemmelshaver
            {
                name = Name,
                email = Email
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Name: '{Name}', " +
                   $"Email: '{Email}'";
        }
    }
}
