namespace Digipost.Api.Client.Common.Print
{
    public interface IPrintIfUnread
    {
        /// <summary>
        ///     The deadline by which the recipient must have read the message, or else it will go to print.
        /// </summary>
        System.DateTime PrintIfUnreadAfter { get; set; }
    
        /// <summary>
        ///     The details for the print fallback, if the recipient did not read the message within the deadline.
        /// </summary>
        IPrintDetails PrintDetails { get; set; }
    }
}
