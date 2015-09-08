using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    /// <summary>
    ///     Optional SMS notification to Recipient.
    ///     Additional charges apply.
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public class SmsNotificationDataTransferObject
    {
        /// <summary>
        ///     Sms notification for a message
        /// </summary>
        /// <param name="afterHours"> Amount of hours untill an SMS will be sent out</param>
        public SmsNotificationDataTransferObject(int afterHours)
        {
            AfterHours = new List<int> {afterHours};
            AtTime = new List<Listedtime>();
        }

        /// <summary>
        ///     Sms notification for a message
        /// </summary>
        /// <param name="sendingTime">The date and time an SMS will be sent out</param>
        public SmsNotificationDataTransferObject(DateTime sendingTime)
        {
            AtTime = new List<Listedtime> {new Listedtime(sendingTime)};
            AfterHours = new List<int>();
        }

        public SmsNotificationDataTransferObject()
        {
            /**must exist for serializing**/
            AfterHours = new List<int>();
            AtTime = new List<Listedtime>();
        }

        [XmlElement("at")]
        public List<Listedtime> AtTime { get; set; }

        [XmlElement("after-hours")]
        public List<int> AfterHours { get; set; }

        public void AddAtTime(Listedtime listedtime)
        {
            AtTime.Add(listedtime);
        }

        public void AddAfterHours(int afterHour)
        {
            AfterHours.Add(afterHour);
        }

        public override string ToString()
        {
            return $"At: {AtTime}, AfterHours: {AfterHours}";
        }
    }
}