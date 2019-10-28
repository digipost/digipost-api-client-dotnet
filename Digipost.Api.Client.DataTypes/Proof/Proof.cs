using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class Proof : IDataType
    {
        /// <summary>
        ///     Name of the authorizer
        /// </summary>
        public string UtstederVisningsnavn { get; set; }
        
        /// <summary>
        ///     Background color of the Proof (#RRGGBB hexcode)
        /// </summary>
        public string BakgrunnsFarge { get; set; }
        
        /// <summary>
        ///     DateTime the Proof was sent
        /// </summary>
        public DateTime UtstedtTidspunkt { get; set; }
        
        /// <summary>
        ///     The valid time period, either single or repeating
        /// </summary>
        public GyldighetsPeriode GyldighetsPeriode { get; set; }
        
        /// <summary>
        ///     The user of the Proof
        /// </summary>
        public Bruker BevisBruker { get; set; }
        
        /// <summary>
        ///     Title of the Proof
        /// </summary>
        public string Tittel { get; set; }
        
        /// <summary>
        ///     Name of the Proof ID
        /// </summary>
        public string BevisIdNavn { get; set; }
        
        /// <summary>
        ///     Value of the Proof ID
        /// </summary>
        public string BevisIdVerdi { get; set; }
        
        /// <summary>
        ///     The Attributes of the Proof
        /// </summary>
        public List<Info> Attributt { get; set; }
        
        /// <summary>
        ///     The Info of the Proof
        /// </summary>
        public List<Info> Info { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal proof AsDataTransferObject()
        {
            var dto = new proof
            {
                utstedervisningsnavn = UtstederVisningsnavn,
                bakgrunnsfarge = BakgrunnsFarge,
                utstedttidspunkt = UtstedtTidspunkt.ToString("O"),
                gyldighetsperioder = GyldighetsPeriode.AsDataTransferObject(),
                bevisbruker = BevisBruker.AsDataTransferObject(),
                tittel = Tittel,
                bevisidnavn = BevisIdNavn,
                bevisidverdi = BevisIdVerdi,
                attributt = Attributt.Select(i => i.AsDataTransferObject()).ToArray(),
                info = Info.Select(i => i.AsDataTransferObject()).ToArray()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"UtstederVisningsnavn: '{UtstederVisningsnavn}', " +
                   $"BakgrunnsFarge: '{BakgrunnsFarge}', " +
                   $"UtstedtTidspunkt: '{UtstedtTidspunkt}', " +
                   $"GyldighetsPeriode: '{GyldighetsPeriode}', " +
                   $"BevisBruker: '{BevisBruker}', " +
                   $"Tittel: '{Tittel}', " +
                   $"BevisIdNavn: '{BevisIdNavn}', " +
                   $"BevisIdVerdi: '{BevisIdVerdi}', " +
                   $"Attributt: '{(Attributt != null ? string.Join(", ", Attributt.Select(x => x.ToString())) : "<none>")}', " +
                   $"Info: '{(Info != null ? string.Join(", ", Info.Select(x => x.ToString())) : "<none>")}'";
        }
    }
}
