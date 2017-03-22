using System;
using System.Collections.Generic;

namespace Digipost.Api.Client.Common
{
    public interface ISmsNotification
    {
        /// <summary>
        ///     List of Listedtime, where each element is the date and time an SMS will be sent out
        /// </summary>
        List<DateTime> NotifyAtTimes { get; set; }

        /// <summary>
        ///     List of integers, where each element is hours after an SMS will be sent out
        /// </summary>
        List<int> NotifyAfterHours { get; set; }
    }
}