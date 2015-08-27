using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Print
{
    public interface IPrintDetails
    {
        /// <summary>
        ///     The recipient of the physical mail.
        /// </summary>
        PrintRecipientDataTransferObject RecipientDataTransferObject { get; set; }

        /// <summary>
        ///     The return address of the physical mail. (if nondeliverable AND the nondeliverable-handling is set to
        ///     ReturnToSender)
        /// </summary>
        PrintReturnRecipientDataTransferObject PrintReturnRecipientDataTransferObject { get; set; }

        /// <summary>
        ///     Defines how fast you want the item delivered. Note: additional charges may apply
        /// </summary>
        PostType PostType { get; set; }

        /// <summary>
        ///     Defines if you want the documents printed in black / white or color (Note: additional charges may apply).
        /// </summary>
        PrintColors Color { get; set; }
    }
}