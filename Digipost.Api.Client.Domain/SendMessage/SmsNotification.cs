using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class SmsNotification : ISmsNotification
    {
        /// <summary>
        /// Sms notification for a message
        /// </summary>
        /// <param name="afterHours"> Amount of hours untill an SMS will be sent out</param>
        public SmsNotification(int afterHours)
        {
            AfterHours = new List<int> { afterHours };
            AtTime = new List<DateTime>();
            
        }

        /// <summary>
        /// Sms notification for a message
        /// </summary>
        /// <param name="sendingTime">The date and time an SMS will be sent out</param>
        public SmsNotification(params DateTime[] sendingTime)
        {
            AtTime = sendingTime.ToList();
            AfterHours = new List<int>();
        }


        /// <summary>
        /// Sms notification for a message
        /// </summary>
        /// <param name="afterHours">List of hours after delivered where the sms notification will be delivered</param>
        public SmsNotification(params int[] afterHours)
        {
            AtTime = new List<DateTime>();
            AfterHours = afterHours.ToList();
        }

        public SmsNotification()
        {
            /**must exist for serializing**/
            AfterHours = new List<int>();
            AtTime = new List<DateTime>();
        }
        public List<DateTime> AtTime { get; set; }
        public List<int> AfterHours { get; set; }

        public void AddAfterHours(int afterHour)
        {
            AfterHours.Add(afterHour);
        }

        public void AddAtTime(DateTime dateTime)
        {
            AtTime.Add(dateTime);
            
        }
    }
}
