using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class MaanedsTidspunkt : IDataType
    {
        public MaanedsTidspunkt(int maaned, int dag)
        {
            Maaned = maaned;
            Dag = dag;
        }

        /// <summary>
        ///     Month (1-12)
        /// </summary>
        public int Maaned { get; set; }
        
        /// <summary>
        ///     Day (1-31)
        /// </summary>
        public int Dag { get; set; }
        
        /// <summary>
        ///     Hour (1-23)
        /// </summary>
        public int Time { get; set; }
        
        /// <summary>
        ///     Minute (1-59)
        /// </summary>
        public int Min { get; set; }
        
        /// <summary>
        ///     Time zone based on the ISO8601 standard
        /// </summary>
        public string Tidssone { get; set; }

        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal maanedsTidspunkt AsDataTransferObject()
        {
            var dto = new maanedsTidspunkt
            {
                maaned = Maaned,
                dag = Dag,
                time = Time,
                min = Min,
                tidssone = Tidssone
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Maaned: '{Maaned}', " +
                   $"Dag: '{Dag}', " +
                   $"Time: '{Time}', " +
                   $"Min: '{Min}', " +
                   $"Tidssone: '{Tidssone}'";
        }
    }
}
