using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <summary>
    ///     Optional SMS notification to Recipient.
    ///     Additional charges apply.
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public class SmsNotification
    {
        /// <summary>
        ///     Amount of hours untill an SMS will be sent out
        /// </summary>
        public SmsNotification(int afterHours)
        {
            SendSmsNotificationAfterHours = new List<int> {afterHours};
            SendSmsNotificationAtTime = new List<Listedtime>();
        }

        /// <summary>
        ///     The date and time an SMS will be sent out
        /// </summary>
        public SmsNotification(DateTime sendingTime)
        {
            SendSmsNotificationAtTime = new List<Listedtime> {new Listedtime(sendingTime)};
            SendSmsNotificationAfterHours = new List<int>();
        }

        private SmsNotification()
        {
            /**must exist for serializing**/
            SendSmsNotificationAfterHours = new List<int>();
            SendSmsNotificationAtTime = new List<Listedtime>();
        }

        /// <summary>
        ///     List of Listedtime, where each element is the date and time an SMS will be sent out
        /// </summary>
        [XmlElement("at")]
        public List<Listedtime> SendSmsNotificationAtTime { get; set; }

        /// <summary>
        ///     List of integers, where each element is hours after an SMS will be sent out
        /// </summary>
        [XmlElement("after-hours")]
        public List<int> SendSmsNotificationAfterHours { get; set; }

        public override string ToString()
        {
            var res = SendSmsNotificationAtTime.Aggregate(" ",
                (current, listedTime) => current + (listedTime.Time.ToString("R")));

            return string.Format("At: {0}, AfterHours: {1}", SendSmsNotificationAtTime, SendSmsNotificationAfterHours);
        }
    }
}