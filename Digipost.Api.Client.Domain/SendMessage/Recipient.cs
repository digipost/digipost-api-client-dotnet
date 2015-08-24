using System;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Recipient : IRecipient
    {
        /// <summary>
        ///     Preferred digital delivery with fallback to physical delivery.
        /// </summary>
        public Recipient(RecipientByNameAndAddress recipientByNameAndAddress, PrintDetails printDetails = null)
        {
            IdentificationValue = recipientByNameAndAddress;
            IdentificationType = IdentificationChoiceType.NameAndAddress;
            PrintDetails = printDetails;
        }

        /// <summary>
        ///     Preferred digital delivery with fallback to physical delivery.
        /// </summary>
        public Recipient(IdentificationChoiceType identificationChoiceType, string id, PrintDetails printDetails = null)
        {
            IdentificationValue = id;
            IdentificationType = identificationChoiceType;
            PrintDetails = printDetails;
        }

        /// <summary>
        ///     Preferred physical delivery. (not Digital)
        /// </summary>
        public Recipient(PrintDetails printDetails)
        {
            PrintDetails = printDetails;
        }

        public object IdentificationValue { get; set; }
        
        public PrintDetails PrintDetails { get; set; }
        
        public IdentificationChoiceType IdentificationType { get; set; }
    }
}
