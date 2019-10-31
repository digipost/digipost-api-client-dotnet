using System.Xml;

namespace Digipost.Api.Client.DataTypes.Proof
{
    public class ProofHolder : IDataType
    {
        public ProofHolder(string firstName, string surname)
        {
            FirstName = firstName;
            Surname = surname;
        }

        /// <summary>
        ///     Name
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        ///     Surname
        /// </summary>
        public string Surname { get; set; }
        
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
        
        internal proofHolder AsDataTransferObject()
        {
            var dto = new proofHolder
            {
                firstname = FirstName,
                surname = Surname,
                socialsecuritynumber = SocialSecurityNumber,
                address = Address?.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Fornavn: '{FirstName}', " +
                   $"Etternavn: '{Surname}', " +
                   $"Foedselsnummer: '{SocialSecurityNumber}', " +
                   $"Address: '{Address?.ToString() ?? "<none>"}'";
        }
    }
}
