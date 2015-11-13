using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Properties;

namespace Digipost.Api.Client
{
    /// <summary>
    ///     Contains configuration for sending digital post. Values can be overridden in App.config , using following format:
    ///     'DP:Variable', where Variable is placeholder for variable name in this class. Changing the timeout
    ///     (TimeoutMilliseconds)
    ///     would result in the following in App.config: <![CDATA[<appSettings><add key="DP:TimeoutMilliseconds" value="10"/></appSettings> ]]>
    /// </summary>
    public class ClientConfig
    {
        private readonly string _senderId = string.Empty;


        /// <summary>
        ///     Client configuration used for setting up the client with settings.
        /// </summary>
        /// <param name="senderId">Defines the id of the sender. If you do not set it here, use App.config. </param>
        /// <param name="apiUrl">The URL for Digipost endpoint</param>
        /// <param name="timeoutInMilliseconds">Timeout intervall for requests made to Digipost, default 30000</param>
        /// <param name="logToFile">Log to file, default false</param>
        /// <param name="logPath">Path to where the logfile wil be placed. Default blank</param>
        public ClientConfig(string senderId, string apiUrl = "https://api.digipost.no/", int timeoutInMilliseconds= 30000, bool logToFile= false, string logPath = "")
        {
            ApiUrl = new Uri(apiUrl);
            TimeoutMilliseconds = timeoutInMilliseconds;
            _senderId = senderId;
            LogToFile = logToFile;
            LogPath = logPath;
        }

        /// <summary>
        ///     Client configuration used for setting up the client with settings.
        /// </summary>
        /// <param name="senderId">Defines the id of the sender. If you do not set it here, use App.config. </param>
        public ClientConfig(string senderId = "")
        {
            ApiUrl = SetFromAppConfig("DP:Url", new Uri(Settings.Default.Url));
            TimeoutMilliseconds = SetFromAppConfig("DP:TimeoutMilliseconds", Settings.Default.TimeoutMilliseconds);

            _senderId = SetFromAppConfig("DP:SenderId", Settings.Default.SenderId);

            if (!string.IsNullOrEmpty(senderId))
                _senderId = senderId;

            LogToFile = SetFromAppConfig("DP:LogToFile", Settings.Default.LogToFile);
            LogPath = SetFromAppConfig("DP:LogPath",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digipost", "Rest",
                    "Log"));
        }

        /// <summary>
        ///     Defines Uri to be used for sending messages. Default value is 'https://api.digipost.no/'. Defines Url to be used
        ///     for message delivery.
        ///     This value can be overridden in the application configuration file with key 'DP:Url' in appSettings.
        /// </summary>
        /// <remarks>
        ///     Url for QA is 'https://qa.api.digipost.no/'.
        /// </remarks>
        public Uri ApiUrl { get; set; }

        /// <summary>
        ///     Defines the timeout for communication with Digipost API. Default is 30 seconds. This
        ///     Angir timeout for komunikasjonen fra og til meldingsformindleren. Default tid er 30 sekunder.
        ///     This value can be overridden in the application configuration file with key 'DP:TimeoutMilliseconds' in
        ///     appSettings.
        /// </summary>
        public int TimeoutMilliseconds { get; set; }

        /// <summary>
        ///     The identification of the technical sender of messages to Digipost. This value is obtained during registration of
        ///     sender.
        /// </summary>
        public string SenderId
        {
            get
            {
                if (string.IsNullOrEmpty(_senderId))
                {
                    throw new ConfigException(
                        "Technical sender id must be valid set to send messages. Set this by code on ClientConfig or in App.config under node" +
                        "'appSettings' with key 'DP:SenderId' (<add key=\"DP:SenderId\" value=\"01234567\"/>)");
                }

                return _senderId;
            }
        }

        /// <summary>
        ///     Exposes logging where you can integrate your own logger or third party logger (i.e. log4net). For use, set an
        ///     anonymous function with
        ///     the following parameters: conversationId, method, message. As a default, trace logging is enabled with
        ///     'Digipost.Api.Client', which can be
        ///     activated in App.config.
        /// </summary>
        public Action<TraceEventType, Guid?, string, string> Logger { get; set; }

        /// <summary>
        ///     Defines if logging is to be done for all messages between the client and Digipost
        /// </summary>
        public bool LogToFile { get; set; }

        /// <summary>
        ///     Defines the path for logging messages sent and received from Digipost API. Default
        ///     path is %Appdata%/Digipost/Rest/Log
        /// </summary>
        public string LogPath { get; set; }

        private T SetFromAppConfig<T>(string key, T @default)
        {
            var appSettings = ConfigurationManager.AppSettings;

            var value = appSettings[key];
            if (value == null)
                return @default;

            if (typeof (IConvertible).IsAssignableFrom(typeof (T)))
            {
                return (T) Convert.ChangeType(value, typeof (T));
            }

            return (T) Activator.CreateInstance(typeof (T), value);
        }
    }
}