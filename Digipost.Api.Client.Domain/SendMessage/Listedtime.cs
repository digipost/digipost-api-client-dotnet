using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    public class Listedtime : IListedtime
    {
        private Listedtime()
        {
            /**Must exist for serialization.**/
        }

        public Listedtime(DateTime time)
        {
            Time = time;
        }
        
        [XmlAttribute("time")]
        public DateTime Time { get; set; }
    }
}