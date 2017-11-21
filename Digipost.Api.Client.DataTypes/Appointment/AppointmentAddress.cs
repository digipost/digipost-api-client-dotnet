using System;

namespace Digipost.Api.Client.DataTypes
{
    public class AppointmentAddress
    {
        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        /// <inheritdoc />
        public AppointmentAddress(string postalCode, string city) : this(string.Empty, postalCode, city)
        {
        }

        public AppointmentAddress(string streetAddress, string postalCode, string city)
        {
            StreetAddress = streetAddress;
            PostalCode = postalCode;
            City = city;
        }
    }
}