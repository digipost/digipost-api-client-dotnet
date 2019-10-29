using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class ValidPeriod : IDataType
    {
        public ValidPeriod(TimePeriode timePeriode)
        {
            TimePeriode = timePeriode;
        }

        /// <summary>
        ///     The valid time period
        /// </summary>
        public TimePeriode TimePeriode { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal gyldighetsPeriode AsDataTransferObject()
        {
            var dto = new gyldighetsPeriode
            {
                Item = TimePeriode
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Tidsperiode: '{TimePeriode}'";
        }
    }
}