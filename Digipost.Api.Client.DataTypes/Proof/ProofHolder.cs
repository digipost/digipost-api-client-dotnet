using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class ProofHolder : IDataType
    {
        public ProofHolder(string firstName, string surName)
        {
            FirstName = firstName;
            SurName = surName;
        }

        /// <summary>
        ///     Name
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        ///     Surname
        /// </summary>
        public string SurName { get; set; }
        
        /// <summary>
        ///     Social Security Number
        /// </summary>
        public string SocialSecurityNumber { get; set; }
        
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
                fornavn = FirstName,
                etternavn = SurName,
                foedselsnummer = SocialSecurityNumber,
                adresse = Address?.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Fornavn: '{FirstName}', " +
                   $"Etternavn: '{SurName}', " +
                   $"Foedselsnummer: '{SocialSecurityNumber}', " +
                   $"Address: '{Address?.ToString() ?? "<none>"}'";
        }
    }
}
