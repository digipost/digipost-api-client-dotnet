using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class Bruker : IDataType
    {
        /// <summary>
        ///     Name
        /// </summary>
        public string Fornavn { get; set; }
        
        /// <summary>
        ///     Surname
        /// </summary>
        public string Etternavn { get; set; }
        
        /// <summary>
        ///     Social Security Number
        /// </summary>
        public string Foedselsnummer { get; set; }
        
        /// <summary>
        ///     Address
        /// </summary>
        public Address Address { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal bruker AsDataTransferObject()
        {
            var dto = new bruker
            {
                fornavn = Fornavn,
                etternavn = Etternavn,
                foedselsnummer = Foedselsnummer,
                adresse = Address.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Fornavn: '{Fornavn}', " +
                   $"Etternavn: '{Etternavn}', " +
                   $"Foedselsnummer: '{Foedselsnummer}', " +
                   $"Address: '{Address}'";
        }
    }
}
