namespace Digipost.Api.Client.Common
{
    /// <summary>
    ///     Contains configuration for sending digital post.
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        ///     Client configuration used for setting up the client with settings.
        /// </summary>
        /// <param name="brokerId">Defines the id of the sender. If you do not set it here, use App.config. </param>
        /// <param name="environment">Environment to connect to for sending.</param>
        public ClientConfig(string brokerId, Environment environment)
        {
            Environment = environment;
            BrokerId = brokerId;
        }

        public Environment Environment { get; set; }

        /// <summary>
        ///     Defines the timeout for communication with Digipost API. Default is 30 seconds.
        /// </summary>
        public int TimeoutMilliseconds { get; set; } = 30000;

        /// <summary>
        ///     The technical sender of messages to Digipost, known as the broker. This value is obtained during registration of
        ///     the broker. If the broker and the sender of the letter are the same organization, this is also the id of the sender.
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        ///     If set to true, all requests and responses are logged with log level DEBUG.
        /// </summary>
        public bool LogRequestAndResponse { get; set; }
    }
}