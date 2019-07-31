using System;

namespace Digipost.Api.Client.Common.Print
{
    public class PrintFallbackDeadline : IPrintFallbackDeadline
    {
        /// <summary>
        ///     The PrintFallbackDeadline to be attached to a Message
        /// </summary>
        /// <param name="deadline">
        ///     The deadline by which the recipient must have read the message, or else it will go to print.
        /// </param>
        /// <param name="printDetails">
        ///     The details for the print fallback, if the recipient did not read the message within the deadline.
        /// </param>
        public PrintFallbackDeadline(DateTime deadline, IPrintDetails printDetails)
        {
            Deadline = deadline;
            PrintDetails = printDetails;
        }
        
        public DateTime Deadline { get; set; }

        public IPrintDetails PrintDetails { get; set; }
    }
}
