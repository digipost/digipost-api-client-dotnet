using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    public class Listedtime
    {
        private Listedtime()
        {
            /**Must exist for serialization.**/
        }

        public Listedtime(DateTime time)
        {
            Time = time;
        }

        /// <summary>
        ///     Date and Time when the sms will be sent out
        /// </summary>
        [XmlAttribute("time")]
        public DateTime Time { get; set; }
        
    }
}