using System;
using System.Collections.Generic;
using System.Linq;

namespace Digipost.Api.Client.Send
{
    public class SmsNotification : ISmsNotification
    {
        public SmsNotification()
        {
            NotifyAfterHours = new List<int>();
            NotifyAtTimes = new List<DateTime>();
        }

        /// <summary>
        ///     Sms notification for a message
        /// </summary>
        /// <param name="sendingTime">The date and time an SMS will be sent out</param>
        public SmsNotification(params DateTime[] sendingTime)
            : this()
        {
            NotifyAtTimes = sendingTime.ToList();
        }

        /// <summary>
        ///     Sms notification for a message
        /// </summary>
        /// <param name="afterHours">List of hours after delivered where the sms notification will be delivered</param>
        public SmsNotification(params int[] afterHours)
            : this()
        {
            NotifyAfterHours = afterHours.ToList();
        }

        public List<DateTime> NotifyAtTimes { get; set; }

        public List<int> NotifyAfterHours { get; set; }
    }
}
