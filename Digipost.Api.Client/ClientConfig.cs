using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Properties;

namespace Digipost.Api.Client
{
    /// <summary>
    /// Contains configuration for sending digital post. Values can be overridden in App.config , using following format: 
    /// 'DP:Variable', where Variable is placeholder for variable name in this class. Changing the timeout (TimeoutMilliseconds)
    /// would result in the following in App.config: <![CDATA[<appSettings><add key="DP:TimeoutMilliseconds" value="10"/></appSettings> ]]>
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        /// Defines Uri to be used for sending messages. Default value is 'https://api.digipost.no/'. Defines Url to be used for message delivery.
        /// This value can be overridden in the application configuration file with key 'DP:Url' in appSettings.
        /// </summary>
        /// <remarks>
        /// Url for QA is 'https://api.digipost.no/'.
        /// </remarks>
        public Uri ApiUrl { get; set; }

        /// <summary>
        /// Defines the timeout for communication with Digipost API. Default is 30 seconds. This 
        /// Angir timeout for komunikasjonen fra og til meldingsformindleren. Default tid er 30 sekunder.
        /// This value can be overridden in the application configuration file with key 'DP:TimeoutMilliseconds' in appSettings.
        /// </summary>
        public int TimeoutMilliseconds { get; set; }

        private string _technicalSenderId = String.Empty;

        /// <summary>
        /// The identification of the technical sender of messages to Digipost. This value is obtained during registration of
        /// sender. 
        /// </summary>
        public string TechnicalSenderId
        {
            get
            {
                if (String.IsNullOrEmpty(_technicalSenderId))
                {
                    throw new ConfigException("Technical sender id must be valid set to send messages. Set this by code on ClientConfig or in App.config under node" +
                                              "'appSettings' with key 'DP:TechnicalSenderId' (<add key=\"DP:TechnicalSenderId\" value=\"01234567\"/>)");
                }

                return _technicalSenderId;
            }
        }

        /// <summary>
        /// Client configuration used for setting up the client with settings.
        /// </summary>
        /// <param name="technicalSenderId">Defines the id of the sender. If you do not set it here, use App.config. </param>
        public ClientConfig(string technicalSenderId = "")
        {
            ApiUrl = SetFromAppConfig<Uri>("DP:Url", new Uri(Settings.Default.Url));
            TimeoutMilliseconds = SetFromAppConfig<int>("DP:TimeoutMilliseconds", Settings.Default.TimeoutMilliseconds);

            _technicalSenderId = SetFromAppConfig<string>("DP:TechnicalSenderId",Settings.Default.TechnicalSenderId);
            
            if (!String.IsNullOrEmpty(technicalSenderId))
                _technicalSenderId = technicalSenderId;

            Logger = Logging.ConsoleLogger();
            LogToFile = SetFromAppConfig<bool>("DP:LogToFile", Settings.Default.LogToFile);
            LogPath = SetFromAppConfig<string>("DP:LogPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digipost", "Rest", "Log"));
        }

        /// <summary>
        /// Exposes logging where you can integrate your own logger or third party logger (i.e. log4net). For use, set an anonymous function with
        /// the following parameters: conversationId, method, message. As a default, trace logging is enabled with 'Digipost.Api.Client', which can be 
        /// activated in App.config.
        /// </summary>
        public Action<TraceEventType, Guid?, string, string> Logger { get; set; }

        /// <summary>
        /// Defines if logging is to be done for all messages between the client and Digipost
        /// </summary>
        public bool LogToFile { get; set; }

        /// <summary>
        /// Defines the path for logging messages sent and received from Digipost API. Default 
        /// path is %Appdata%/Digipost/Rest/Log
        /// </summary>
        public string LogPath { get; set; }

        private T SetFromAppConfig<T>(string key, T @default)
        {
            var appSettings = ConfigurationManager.AppSettings;

            string value = appSettings[key];
            if (value == null)
                return @default;

            if (typeof(IConvertible).IsAssignableFrom(typeof(T)))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            
            return (T)Activator.CreateInstance(typeof(T), value);
        }
    }
}
