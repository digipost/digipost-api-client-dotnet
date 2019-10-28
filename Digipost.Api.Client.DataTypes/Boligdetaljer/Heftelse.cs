using System.Xml;

namespace Digipost.Api.Client.DataTypes.Boligdetaljer
{
    public class Heftelse : IDataType
    {
        public Heftelse(string pantHaver, string typePant, string beloep)
        {
            PantHaver = pantHaver;
            TypePant = typePant;
            Beloep = beloep;
        }

        /// <summary>
        ///     
        /// </summary>
        public string PantHaver { get; set; }
        
        /// <summary>
        ///     
        /// </summary>
        public string TypePant { get; set; }
        
        /// <summary>
        ///     
        /// </summary>
        public string Beloep { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal heftelse AsDataTransferObject()
        {
            var dto = new heftelse
            {
                panthaver = PantHaver,
                typepant = TypePant,
                beloep = Beloep
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"PantHaver: '{PantHaver}', " +
                   $"TypePant: '{TypePant}', " +
                   $"Beloep: '{Beloep}'";
        }
    }
}
