using System;
using Digipost.Api.Client.Common.Print;

namespace Digipost.Api.Client.Send
{
    public class RequestForRegistration
    {
        public RequestForRegistration(
            DateTime registrationDeadline,
            String phoneNumber,
            String emailAddress,
            IPrintDetails printDetails
        )
        {
            RegistrationDeadline = registrationDeadline;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            PrintDetails = printDetails;
        }

        public DateTime RegistrationDeadline { get; }

        public string PhoneNumber { get; }

        public string EmailAddress { get; }

        public IPrintDetails PrintDetails { get; }
    }
}
