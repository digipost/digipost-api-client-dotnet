namespace Digipost.Api.Client.Common.Enums
{
    /// <summary>
    ///     Defines if the message is sensitive or not.
    /// </summary>
    public enum SensitivityLevel
    {
        /// <summary>
        ///     Default. Non sensitive message. Metadata about the message, like the sender and subject,
        ///     will be revealed in user notifications (eg. email and SMS), and can also be seen when logged in at a
        ///     security level below the one specified for the message.
        /// </summary>
        Normal,

        /// <summary>
        ///     Sensitive message. Metadata about the message, like the sender and subject, will be hidden
        ///     until logged in at the appropriate security level specified for the message.
        /// </summary>
        Sensitive
    }
}
