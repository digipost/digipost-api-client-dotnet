using System;
using System.Collections.Generic;

namespace Digipost.Api.Client.Send
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

        /// <summary>
        ///     If true, the SMS notification will be sent even if the recipient has reserved themselves against
        ///     receiving sender-initiated SMS notifications from Digipost. It will also be sent even if the recipient
        ///     has already read the message before the scheduled notification time - normally a pending notification
        ///     is cancelled as soon as the message is read, but that cancellation is skipped when this is set.
        ///     Requires special permission for the sending organisation - using this while not being permitted to
        ///     will result in an error response.
        /// </summary>
        bool AlwaysSend { get; set; }
    }
}
