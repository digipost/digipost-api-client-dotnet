using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    public class ListedTimeDataTransferObject 
    {
        private ListedTimeDataTransferObject()
        {
            /**Must exist for serialization.**/
        }

        public ListedTimeDataTransferObject(DateTime time)
        {
            Time = time;
        }
        
        [XmlAttribute("time")]
        public DateTime Time { get; set; }
    }
}