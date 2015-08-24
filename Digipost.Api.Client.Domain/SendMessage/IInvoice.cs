using System;

namespace Digipost.Api.Client.Domain
{
    public interface IInvoice
    {
        /// <summary>
        /// Customer identification number. 2 to 25 digits with no spaces or dots. Mandatory by default.  
        /// </summary>
        string Kid { get; set; }

        /// <summary>
        /// The amount of the invoice.
        /// </summary>
        decimal Amount { get; set; }

        /// <summary>
        /// Receiving account. 11 digits with no spaces or dots.
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// When the invoice is due.
        /// </summary>
        DateTime Duedate { get; set; }
    }
}