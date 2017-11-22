namespace Digipost.Api.Client.DataTypes
{
    public class AppointmentAddress
    {
        /// <inheritdoc />
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

        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }

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
    }
}
