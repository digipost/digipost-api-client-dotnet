using System;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IRecipientByNameAndAddress
    {
        string FullName { get; set; }

        string AddressLine1 { get; set; }

        string AddressLine2 { get; set; }

        string PostalCode { get; set; }

        string City { get; set; }

        DateTime? BirthDate { get; set; }

        string PhoneNumber { get; set; }

        string Email { get; set; }
    }
}