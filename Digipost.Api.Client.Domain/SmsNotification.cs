using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public class SmsNotification
    {

        public SmsNotification(int afterHours)
        {
            AfterHours = new List<int> {afterHours};
        }

        public SmsNotification(DateTime sendingTime)
        {
            At = new List<Listedtime> {new Listedtime(sendingTime)};
        }

        private SmsNotification()
        {
            AfterHours = new List<int>();
            At = new List<Listedtime>();
        }

        /// <summary>
        ///     List of integers, where each element is hours after an SMS will be sent out
        /// </summary>
        [XmlElement("at")]
        public List<Listedtime> At { get; set; }

        [XmlElement("after-hours")]
        public List<int> AfterHours { get; set; }
    }

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