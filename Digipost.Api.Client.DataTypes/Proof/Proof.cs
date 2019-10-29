using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class Proof : IDataType
    {
        public Proof(string authorizerName, ValidPeriod validPeriod, ProofHolder bevisProofHolder, string title)
        {
            AuthorizerName = authorizerName;
            ValidPeriod = validPeriod;
            BevisProofHolder = bevisProofHolder;
            Title = title;
        }

        /// <summary>
        ///     Name of the authorizer
        /// </summary>
        public string AuthorizerName { get; set; }
        
        /// <summary>
        ///     Background color of the Proof (#RRGGBB hexcode)
        /// </summary>
        public string BackgroundColor { get; set; }
        
        /// <summary>
        ///     DateTime the Proof was sent
        /// </summary>
        public DateTime? IssuedTime { get; set; }
        
        /// <summary>
        ///     The valid time period, either single or repeating
        /// </summary>
        public ValidPeriod ValidPeriod { get; set; }
        
        /// <summary>
        ///     The user of the Proof
        /// </summary>
        public ProofHolder BevisProofHolder { get; set; }
        
        /// <summary>
        ///     Title of the Proof
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        ///     Name of the Proof ID
        /// </summary>
        public string ProofIdName { get; set; }
        
        /// <summary>
        ///     Value of the Proof ID
        /// </summary>
        public string ProofIdValue { get; set; }
        
        /// <summary>
        ///     The Attributes of the Proof
        /// </summary>
        public List<Info> Attribute { get; set; }
        
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
                utstedervisningsnavn = AuthorizerName,
                bakgrunnsfarge = BackgroundColor,
                utstedttidspunkt = IssuedTime?.ToString("O"),
                gyldighetsperioder = ValidPeriod.AsDataTransferObject(),
                bevisbruker = BevisProofHolder.AsDataTransferObject(),
                tittel = Title,
                bevisidnavn = ProofIdName,
                bevisidverdi = ProofIdValue,
                attributt = Attribute?.Select(i => i.AsDataTransferObject()).ToArray(),
                info = Info?.Select(i => i.AsDataTransferObject()).ToArray()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"UtstederVisningsnavn: '{AuthorizerName}', " +
                   $"BakgrunnsFarge: '{BackgroundColor}', " +
                   $"UtstedtTidspunkt: '{IssuedTime?.ToString("O") ?? "<none>"}', " +
                   $"GyldighetsPeriode: '{ValidPeriod}', " +
                   $"BevisBruker: '{BevisProofHolder}', " +
                   $"Tittel: '{Title}', " +
                   $"BevisIdNavn: '{ProofIdName}', " +
                   $"BevisIdVerdi: '{ProofIdValue}', " +
                   $"Attributt: '{(Attribute != null ? string.Join(", ", Attribute.Select(x => x.ToString())) : "<none>")}', " +
                   $"Info: '{(Info != null ? string.Join(", ", Info.Select(x => x.ToString())) : "<none>")}'";
        }
    }
}
