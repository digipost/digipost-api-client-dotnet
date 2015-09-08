using System;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IRecipientByNameAndAddress
    {
        /// <summary>
        ///     Full name of person, first name first.
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        ///     Primary address
        /// </summary>
        string AddressLine1 { get; set; }

        /// <summary>
        ///     Secondary addressline
        /// </summary>
        string AddressLine2 { get; set; }

        /// <summary>
        ///     Postal code
        /// </summary>
        string PostalCode { get; set; }

        /// <summary>
        ///     City
        /// </summary>
        string City { get; set; }

        /// <summary>
        ///     Birth date
        /// </summary>
        DateTime? BirthDate { get; set; }

        /// <summary>
        ///     Phone number
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        ///     E-mail address
        /// </summary>
        string Email { get; set; }
    }
}