using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.SendMessage
{
    /// <summary>
    ///     Optional SMS notification to Recipient.
    ///     Additional charges apply.
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public class SmsNotification : ISmsNotification
    {
        /// <summary>
        /// Sms notification for a message
        /// </summary>
        /// <param name="afterHours"> Amount of hours untill an SMS will be sent out</param>
        public SmsNotification(int afterHours)
        {
            AddAfterHours = new List<int> {afterHours};
            AddAtTime = new List<Listedtime>();
        }

        /// <summary>
        /// Sms notification for a message
        /// </summary>
        /// <param name="sendingTime">The date and time an SMS will be sent out</param>
        public SmsNotification(DateTime sendingTime)
        {
            AddAtTime = new List<Listedtime> {new Listedtime(sendingTime)};
            AddAfterHours = new List<int>();
        }

        private SmsNotification()
        {
            /**must exist for serializing**/
            AddAfterHours = new List<int>();
            AddAtTime = new List<Listedtime>();
        }

        [XmlElement("at")]
        public List<Listedtime> AddAtTime { get; set; }

        [XmlElement("after-hours")]
        public List<int> AddAfterHours { get; set; }

        public override string ToString()
        {
            var res = AddAtTime.Aggregate(" ",
                (current, listedTime) => current + (listedTime.Time.ToString("R")));

            return string.Format("At: {0}, AfterHours: {1}", AddAtTime, AddAfterHours);
        }
    }
}