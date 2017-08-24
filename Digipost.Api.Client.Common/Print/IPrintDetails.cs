using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Print
{
    public interface IPrintDetails
    {
        /// <summary>
        ///     The recipient of the physical mail.
        /// </summary>
        IPrintRecipient PrintRecipient { get; set; }

        /// <summary>
        ///     The return address of the physical mail. (if nondeliverable AND the nondeliverable-handling is set to
        ///     ReturnToSender)
        /// </summary>
        IPrintReturnRecipient PrintReturnRecipient { get; set; }

        /// <summary>
        ///     Defines how fast you want the item delivered. Note: additional charges may apply
        /// </summary>
        PostType PostType { get; set; }

        /// <summary>
        ///     Defines if you want the documents printed in black / white or color (Note: additional charges may apply).
        /// </summary>
        PrintColors PrintColors { get; set; }

        /// <summary>
        ///     Determines the exception handling that will occur when the letter can not be delivered.
        /// </summary>
        NondeliverableHandling NondeliverableHandling { get; }
    }
}
