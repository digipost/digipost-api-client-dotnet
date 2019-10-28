using System.Xml;

namespace Digipost.Api.Client.DataTypes.Boligdetaljer
{
    public class Residence : IDataType
    {
        public Residence(ResidenceAddress address)
        {
            Address = address;
        }

        /// <summary>
        ///     Residence address
        /// </summary>
        public ResidenceAddress Address { get; set; }
        
        /// <summary>
        ///     Matrikkel
        /// </summary>
        public Matrikkel Matrikkel { get; set; }
        
        /// <summary>
        ///     Source
        /// </summary>
        public string Source { get; set; }
        
        /// <summary>
        ///     External ID
        /// </summary>
        public string ExternalId { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal residence AsDataTransferObject()
        {
            var dto = new residence
            {
                address = Address.AsDataTransferObject(),
                matrikkel = Matrikkel?.AsDataTransferObject(),
                source = Source,
                externalid = ExternalId
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Address: '{Address}', " +
                   $"Matrikkel: '{Matrikkel?.ToString() ?? "<none>"}', " +
                   $"Source: '{Source}', " +
                   $"ExternalId: '{ExternalId}'";
        }
    }
}
