using System;

namespace Digipost.Api.Client.Common.Print
{
    public class PrintIfUnread : IPrintIfUnread
    {
        /// <summary>
        ///     The PrintFallbackDeadline to be attached to a Message
        /// </summary>
        /// <param name="printifunreadafter">
        ///     The deadline by which the recipient must have read the message, or else it will go to print.
        /// </param>
        /// <param name="printDetails">
        ///     The details for the print fallback, if the recipient did not read the message within the deadline.
        /// </param>
        public PrintIfUnread(DateTime printifunreadafter, IPrintDetails printDetails)
        {
            PrintIfUnreadAfter = printifunreadafter;
            PrintDetails = printDetails;
        }
        
        public DateTime PrintIfUnreadAfter { get; set; }

        public IPrintDetails PrintDetails { get; set; }
    }
}
