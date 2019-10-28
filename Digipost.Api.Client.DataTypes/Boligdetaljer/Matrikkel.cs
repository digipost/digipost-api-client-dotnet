using System.Xml;

namespace Digipost.Api.Client.DataTypes.Boligdetaljer
{
    public class Matrikkel : IDataType
    {
        /// <summary>
        ///     Kommunenummer
        /// </summary>
        public string KommuneNummer { get; set; }
        
        /// <summary>
        ///     Gaardsnummer
        /// </summary>
        public string GaardsNummer { get; set; }
        
        /// <summary>
        ///     Bruksnummer
        /// </summary>
        public string BruksNummer { get; set; }
        
        /// <summary>
        ///     Festenummer
        /// </summary>
        public string FesteNummer { get; set; }
        
        /// <summary>
        ///     Seksjonsnummer
        /// </summary>
        public string SeksjonsNummer { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal matrikkel AsDataTransferObject()
        {
            var dto = new matrikkel
            {
                kommunenummer = KommuneNummer,
                gaardsnummer = GaardsNummer,
                bruksnummer = BruksNummer,
                festenummer = FesteNummer,
                seksjonsnummer = SeksjonsNummer
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"KommuneNummer: '{KommuneNummer}', " +
                   $"GaardsNummer: '{GaardsNummer}', " +
                   $"BruksNummer: '{BruksNummer}', " +
                   $"FesteNummer: '{FesteNummer}', " +
                   $"SeksjonsNummer: '{SeksjonsNummer}'";
        }
    }
}
