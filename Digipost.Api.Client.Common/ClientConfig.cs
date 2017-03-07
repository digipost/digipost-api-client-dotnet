namespace Digipost.Api.Client
{
    /// <summary>
    ///     Contains configuration for sending digital post.
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        ///     Client configuration used for setting up the client with settings.
        /// </summary>
        /// <param name="senderId">Defines the id of the sender. If you do not set it here, use App.config. </param>
        /// <param name="environment">Environment to connect to for sending.</param>
        public ClientConfig(string senderId, Environment environment)
        {
            Environment = environment;
            SenderId = senderId;
        }

        public Environment Environment { get; set; }

        /// <summary>
        ///     Defines the timeout for communication with Digipost API. Default is 30 seconds.
        /// </summary>
        public int TimeoutMilliseconds { get; set; } = 30000;

        /// <summary>
        ///     The identification of the technical sender of messages to Digipost. This value is obtained during registration of
        ///     sender.
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        ///     If set to true, all requests and responses are logged with log level DEBUG.
        /// </summary>
        public bool LogRequestAndResponse { get; set; }
    }
}