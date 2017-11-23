namespace Digipost.Api.Client.DataTypes
{
    public class AppointmentAddress
    {
        public AppointmentAddress(string postalCode, string city)
            : this(null, postalCode, city)
        {
        }

        public AppointmentAddress(string streetAddress, string postalCode, string city)
        {
            StreetAddress = streetAddress;
            PostalCode = postalCode;
            City = city;
        }

        /// <summary>
        ///     Optional street address. 100 characters or less.
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        ///     10 characters or less.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        ///     100 characters or less.
        /// </summary>
        public string City { get; set; }

        internal appointmentAddress AsDataTransferObject()
        {
            return new appointmentAddress
            {
                streetaddress = StreetAddress,
                postalcode = PostalCode,
                city = City
            };
        }

        public override string ToString()
        {
            return $"Address: '{(StreetAddress != null ? $"{StreetAddress}, " : "")}{PostalCode} {City}'";
        }
    }
}
