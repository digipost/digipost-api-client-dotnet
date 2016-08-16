using System;
using System.Diagnostics;

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
        /// <param name="apiUrl">The URL for Digipost endpoint</param>
        /// <param name="timeoutInMilliseconds">Timeout intervall for requests made to Digipost, default 30000</param>
        /// <param name="logToFile">Log to file, default false</param>
        /// <param name="logPath">Path to where the logfile wil be placed. Default blank</param>
        public ClientConfig(string senderId, string apiUrl = "https://api.digipost.no/", int timeoutInMilliseconds = 30000, bool logToFile = false, string logPath = "")
        {
            ApiUrl = new Uri(apiUrl);
            TimeoutMilliseconds = timeoutInMilliseconds;
            SenderId = senderId;
            LogToFile = logToFile;
            LogPath = logPath;
        }

        /// <summary>
        ///     Defines Uri to be used for sending messages. Default value is 'https://api.digipost.no/'. Defines Url to be used
        ///     for message delivery.
        /// </summary>
        /// <remarks>
        ///     Url for QA is 'https://qa.api.digipost.no/'.
        /// </remarks>
        public Uri ApiUrl { get; set; }

        /// <summary>
        ///     Defines the timeout for communication with Digipost API. Default is 30 seconds.
        /// </summary>
        public int TimeoutMilliseconds { get; set; }

        /// <summary>
        ///     The identification of the technical sender of messages to Digipost. This value is obtained during registration of
        ///     sender.
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        ///     Exposes logging where you can integrate your own logger or third party logger (i.e. log4net). For use, set an
        ///     anonymous function with the following parameters: conversationId, method, message. As a default, trace logging is enabled with
        ///     'Digipost.Api.Client', which can be
        ///     activated in App.config.
        /// </summary>
        public Action<TraceEventType, Guid?, string, string> Logger { get; set; }

        /// <summary>
        ///     Defines if logging is to be done for all messages between the client and Digipost.
        /// </summary>
        public bool LogToFile { get; set; }

        /// <summary>
        ///     Defines the path for logging messages sent and received from Digipost API. Default
        ///     path is %Appdata%/Digipost/Rest/Log
        /// </summary>
        public string LogPath { get; set; }

    }
}