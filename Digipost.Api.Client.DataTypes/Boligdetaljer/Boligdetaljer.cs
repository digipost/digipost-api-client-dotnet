using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Boligdetaljer
{
    public class Boligdetaljer : IDataType
    {
        public Boligdetaljer(Residence residence)
        {
            Residence = residence;
        }

        /// <summary>
        ///     Residence
        /// </summary>
        public Residence Residence { get; set; }
        
        /// <summary>
        ///     List of people with legal rights associated with the residence
        /// </summary>
        public List<Hjemmelshaver> Hjemmelshavere { get; set; }
        
        /// <summary>
        ///     Size (Square meters) of the Residence
        /// </summary>
        public int? BruksAreal { get; set; }
        
        /// <summary>
        ///     Numbers of rooms in the Residence, not including Bathrooms, Kitchen or Storage Rooms
        /// </summary>
        public int? AntallOppholdsRom { get; set; }
        
        /// <summary>
        ///     Number of Bathrooms in the Residence
        /// </summary>
        public int? AntallBaderom { get; set; }
        
        /// <summary>
        ///     Info about previous sales and transactions regarding the Residence
        /// </summary>
        public List<Omsetningshistorikk> Omsetningshistorikk { get; set; }
        
        /// <summary>
        ///     Organization number
        /// </summary>
        public string OrganisasjonsNummer { get; set; }
        
        /// <summary>
        ///     Residence number. Must be of format [UKHL]0000. E.g. H0304
        /// </summary>
        public string BruksEnhet { get; set; }
        
        /// <summary>
        ///     
        /// </summary>
        public string AndelsNummber { get; set; }
        
        /// <summary>
        ///     
        /// </summary>
        public List<Heftelse> Heftelser { get; set; }
        
        /// <summary>
        ///     An optional ExternalLink prompting the user to perform an action on an external site
        /// </summary>
        public ExternalLink CallToAction { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal boligdetaljer AsDataTransferObject()
        {
            var dto = new boligdetaljer
            {
                residence = Residence.AsDataTransferObject(),
                hjemmelshavere = Hjemmelshavere?.Select(i => i.AsDataTransferObject()).ToArray(),
                bruksareal = BruksAreal.GetValueOrDefault(0),
                bruksarealSpecified = BruksAreal.HasValue,
                antalloppholdsrom = AntallOppholdsRom.GetValueOrDefault(0),
                antalloppholdsromSpecified = AntallOppholdsRom.HasValue,
                antallbaderom = AntallBaderom.GetValueOrDefault(0),
                antallbaderomSpecified = AntallBaderom.HasValue,
                omsetningshistorikk = Omsetningshistorikk?.Select(i => i.AsDataTransferObject()).ToArray(),
                organisasjonsnummer = OrganisasjonsNummer,
                bruksenhet = BruksEnhet,
                andelsnummer = AndelsNummber,
                heftelser = Heftelser?.Select(i => i.AsDataTransferObject()).ToArray(),
                callToAction = CallToAction?.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Residence: '{Residence}', " +
                   $"Hjemmelshavere: '{(Hjemmelshavere != null ? string.Join(", ", Hjemmelshavere.Select(x => x.ToString())) : "<none>")}', " +
                   $"BruksAreal: '{BruksAreal}', " +
                   $"AntallOppholdsRom: '{AntallOppholdsRom}', " +
                   $"AntallBaderom: '{AntallBaderom}', " +
                   $"Omsetningshistorikk: '{(Omsetningshistorikk != null ? string.Join(", ", Omsetningshistorikk.Select(x => x.ToString())) : "<none>")}', " +
                   $"OrganisasjonsNummer: '{OrganisasjonsNummer}', " +
                   $"BruksEnhet: '{BruksEnhet}', " +
                   $"AndelsNummber: '{AndelsNummber}', " +
                   $"Heftelser: '{(Heftelser != null ? string.Join(", ", Heftelser.Select(x => x.ToString())) : "<none>")}', " +
                   $"CallToAction: '{CallToAction?.ToString() ?? "<none>"}'";
        }
    }
}
