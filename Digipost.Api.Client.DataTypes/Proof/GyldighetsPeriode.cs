using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class GyldighetsPeriode : IDataType
    {
        /// <summary>
        ///     The valid time period
        /// </summary>
        public TidsPeriode TidsPeriode { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal gyldighetsPeriode AsDataTransferObject()
        {
            var dto = new gyldighetsPeriode
            {
                Item = TidsPeriode
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Tidsperiode: '{TidsPeriode}'";
        }
    }
}
