using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Digipost.Api.Client.Digipost.Api.Client;
using Digipost.Api.Client.Domain.Exceptions;

namespace Digipost.Api.Client
{
    /// <summary>
    /// Contains configuration for sending digital post. Values can be overridden in App.config , using following format: 
    /// 'DP:Variable', where
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        /// Defines Uri to be used for sending messages. Default value is 'https://api.digipost.no/'. 
        /// Angir Uri som skal benyttes for sending av meldinger. Standardverdi er 'https://api.digipost.no/'. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:MeldingsformidlerUrl'.
        /// </summary>
        /// <remarks>
        /// Uri for QA miljø er 'https://qaoffentlig.meldingsformidler.digipost.no/api/ebms'.
        /// </remarks>
        public Uri ApiUrl { get; set; }

        /// <summary>
        /// Angir timeout for komunikasjonen fra og til meldingsformindleren. Default tid er 30 sekunder. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:TimeoutIMillisekunder'.
        /// </summary>
        public int TimeoutMilliseconds { get; set; }

        private string _technicalSenderId = String.Empty;

        /// <summary>
        /// The id of the technical sender of messages to Digipost
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
        /// Exposes logging where you can integrate your own logger or third party logger (i.e. log4net). For use, set an anonymous function with
        /// the following parameters: conversationId, method, message. As a default, trace logging is enabled with 'Digipost.Api.Client', which can be 
        /// activated in App.config.
        /// </summary>
        public Action<TraceEventType, Guid?, string, string> Logger { get; set; }

        /// <summary>
        /// Defines if logging is to be done for all messages between the client and Digipost
        /// </summary>
        public bool LogToFile { get; set; }

        public string LogPath { get; set; }

        /// <summary>
        /// Client configuration used for setting up the client with settings as timeout, Url and logging.
        /// </summary>
        /// <param name="technicalSenderId">Defines the id of the sender. If you do not set it here, use App.config. </param>
        public ClientConfig(string technicalSenderId = "")
        {
            ApiUrl = SetFromAppConfig<Uri>("DP:Url", new Uri(Properties.Settings.Default.Url));
            TimeoutMilliseconds = SetFromAppConfig<int>("DP:TimeoutMilliseconds", Properties.Settings.Default.TimeoutMilliseconds);

            if (String.IsNullOrEmpty(technicalSenderId))
            {
                _technicalSenderId = SetFromAppConfig<string>("DP:TechnicalSenderId",
                    Properties.Settings.Default.TechnicalSenderId);
            }
            
            Logger = Logging.TraceLogger();
            LogToFile = SetFromAppConfig<bool>("DP:LogToFile", Properties.Settings.Default.LogToFile);
            LogPath = SetFromAppConfig<string>("DP:LogPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digipost", "Rest", "Log"));
        }
        
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
