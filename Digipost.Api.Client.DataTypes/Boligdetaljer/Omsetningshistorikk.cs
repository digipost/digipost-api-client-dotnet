using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Boligdetaljer
{
    public class Omsetningshistorikk : IDataType
    {
        public Omsetningshistorikk(DateTime dato)
        {
            Dato = dato;
        }

        /// <summary>
        ///     ISO8691 full DateTime
        /// </summary>
        public DateTime Dato { get; set; }
        
        /// <summary>
        ///     Beskrivelse
        /// </summary>
        public string Beskrivelse { get; set; }
        
        /// <summary>
        ///     Selger
        /// </summary>
        public string Selger { get; set; }
        
        /// <summary>
        ///     Kjoeper
        /// </summary>
        public string Kjoeper { get; set; }
        
        /// <summary>
        ///     Beloep
        /// </summary>
        public long? Beloep { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal omsetningshistorikk AsDataTransferObject()
        {
            var dto = new omsetningshistorikk
            {
                dato = Dato.ToString("O"),
                beskrivelse = Beskrivelse,
                selger = Selger,
                kjoeper = Kjoeper,
                beloep = Beloep.GetValueOrDefault(0),
                beloepSpecified = Beloep.HasValue
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Dato: '{Dato}', " +
                   $"Beskrivelse: '{Beskrivelse}', " +
                   $"Selger: '{Selger}', " +
                   $"Kjoeper: '{Kjoeper}', " +
                   $"Beloep: '{Beloep}'";
        }
    }
}
