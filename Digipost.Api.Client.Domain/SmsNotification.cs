using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <summary>
    /// Optional SMS notification to Recipient.
    /// Additional charges apply.
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public class SmsNotification
    {
        /// <summary>
        ///    Amount of hours untill an SMS will be sent out
        /// </summary>
        public SmsNotification(int afterHours)
        {
            AfterHours = new List<int> {afterHours};
            At = new List<Listedtime>();
        }

        /// <summary>
        ///    The date and time an SMS will be sent out
        /// </summary>
        public SmsNotification(DateTime sendingTime)
        {
            At = new List<Listedtime> {new Listedtime(sendingTime)};
            AfterHours =  new List<int>();
        }

        private SmsNotification()
        {
            /**must exist for serializing**/
            AfterHours = new List<int>();
            At = new List<Listedtime>();
        }

        /// <summary>
        ///     List of Listedtime, where each element is the date and time an SMS will be sent out
        /// </summary>
        [XmlElement("at")]
        public List<Listedtime> At { get; set; }

        /// <summary>
        ///     List of integers, where each element is hours after an SMS will be sent out
        /// </summary>
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