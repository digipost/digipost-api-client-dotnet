using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class AarligRepeterendePeriode : IDataType
    {
        /// <summary>
        ///     Starting year of the repeating period
        /// </summary>
        public int StartAar { get; set; }
        
        /// <summary>
        ///     Ending year of the repeating period
        /// </summary>
        public int SluttAar { get; set; }
        
        /// <summary>
        ///     Starting month of the repeating period
        /// </summary>
        public MaanedsTidspunkt Fra { get; set; }
        
        /// <summary>
        ///     Ending month of the repeating period
        /// </summary>
        public MaanedsTidspunkt Til { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal aarligRepeterendePeriode AsDataTransferObject()
        {
            var dto = new aarligRepeterendePeriode
            {
                startaar = StartAar,
                sluttaar = SluttAar,
                fra = Fra.AsDataTransferObject(),
                til = Til.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"StartAar: '{StartAar}', " +
                   $"SluttAar: '{SluttAar}', " +
                   $"Fra: '{Fra.ToString()}', " +
                   $"Til: '{Til.ToString()}'";
        }
    }
}
