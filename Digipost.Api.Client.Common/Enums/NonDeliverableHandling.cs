namespace Digipost.Api.Client.Common.Enums
{
    public enum NondeliverableHandling
    {
        /// <summary>
        ///     If mail is undeliverable the mail will be returned to the return address.
        /// </summary>
        ReturnToSender,
        /// <summary>
        ///     If mail is undeliverable the mail will be shredded by the mail provider.
        /// </summary>
        Shred
    }
}
