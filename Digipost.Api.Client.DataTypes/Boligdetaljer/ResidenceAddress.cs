using System.Xml;

namespace Digipost.Api.Client.DataTypes
{
    public class ResidenceAddress : IDataType
    {
        /// <summary>
        ///     Bolignummer. Must be of format [UKHL]0000. E.g. H0304
        /// </summary>
        public string UnitNumber { get; set; }
        
        /// <summary>
        ///     A house number with or without a house letter. E.g. 11 or 11A
        /// </summary>
        public string HouseNumber { get; set; }
        
        /// <summary>
        ///     The name of the street. E.g. Storgata
        /// </summary>
        public string StreetName { get; set; }
        
        /// <summary>
        ///     Postal code
        /// </summary>
        public string PostalCode { get; set; }
        
        /// <summary>
        ///     City
        /// </summary>
        public string City { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal residenceAddress AsDataTransferObject()
        {
            var dto = new residenceAddress
            {
                unitnumber = UnitNumber,
                housenumber = HouseNumber,
                streetname = StreetName,
                postalcode = PostalCode,
                city = City
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"UnitNumber: '{UnitNumber}', " +
                   $"HouseNumber: '{HouseNumber}', " +
                   $"StreetName: '{StreetName}', " +
                   $"PostalCode: '{PostalCode}', " +
                   $"City: '{City}'";
        }
    }
}
