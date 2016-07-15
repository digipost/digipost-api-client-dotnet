using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public class SmsNotificationDataTransferObject
    {
        public SmsNotificationDataTransferObject()
        {
            NotifyAfterHours = new List<int>();
            NotifyAtTimes = new List<ListedTimeDataTransferObject>();
        }

        public SmsNotificationDataTransferObject(int afterHours)
            : this()
        {
            NotifyAfterHours.Add(afterHours);
        }

        public SmsNotificationDataTransferObject(DateTime sendingTime)
            : this()
        {
            NotifyAtTimes.Add(new ListedTimeDataTransferObject(sendingTime));
        }

        [XmlElement("at")]
        public List<ListedTimeDataTransferObject> NotifyAtTimes { get; set; }

        [XmlElement("after-hours")]
        public List<int> NotifyAfterHours { get; set; }

        public override string ToString()
        {
            return string.Format("AtTime: {0}, AfterHours: {1}", NotifyAtTimes, NotifyAfterHours);
        }
    }
}