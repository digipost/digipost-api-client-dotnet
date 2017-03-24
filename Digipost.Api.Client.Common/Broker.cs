namespace Digipost.Api.Client.Common
{
    /// <summary>
    ///     The actual sender of the message. The broker is the owner of the organization certificate used in the client
    ///     library.
    /// </summary>
    public class Broker
    {
        /// <param name="id">
        ///     The id for actual sender of the message. If used in scenarios where one party, the broker, is
        ///     creating a message on behalf of another, the sender, this id is the id of the broker. Otherwise, this will be the
        ///     same id as used for the sender.
        /// </param>
        public Broker(long id)
        {
            Id = id;
        }

        /// <summary>
        ///     The id for actual sender of the message. If used in scenarios where one party, the broker, is creating a message
        ///     on behalf of another, the sender, this id is the id of the broker. Otherwise, this will be the same id as used
        ///     for the sender.
        /// </summary>
        public long Id { get; set; }
    }
}