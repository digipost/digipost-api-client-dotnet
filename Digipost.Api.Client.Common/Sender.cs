namespace Digipost.Api.Client.Common
{
    /// <summary>
    ///     The sender of the message, i.e. what the receiver of the message sees as the sender of the message.
    ///     If you are delivering a message on behalf of an organization with id 5555, set this property
    ///     to 5555. If you are delivering on behalf of yourself, set this to your organization`s sender id.
    ///     The id is created by Digipost.
    /// </summary>
    public class Sender
    {
        /// <summary>
        ///     The sender of the message, i.e. what the receiver of the message sees as the sender of the message.
        /// </summary>
        /// <param name="id">The id of the sender, created by Digipost</param>
        public Sender(long id)
        {
            Id = id;
        }

        /// <summary>
        ///     The id of the sender of the message, i.e. what the receiver of the message sees as the sender of the message. If
        ///     you are delivering a message on behalf of an organization with id 5555, set this property to 5555. If you are
        ///     delivering on behalf of yourself, set this to your organization`s sender id. The id is created by Digipost.
        /// </summary>
        public long Id { get; }
    }
}
