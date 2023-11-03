namespace Digipost.Api.Client.Common.SenderInfo
{
    public enum SenderStatus
    {
        /// <summary>
        /// No information about the requested sender could be retrieved. This may either be because the
        /// sender does not exist, or the broker is not authorized to send on behalf of the sender.
        /// </summary>
        NoInfoAvailable,

        /// <summary>
        /// The sender exists in Digipost, and the broker is authorized to act on behalf of
        /// the sender (typically send messages).
        /// </summary>
        ValidSender
    }
}
