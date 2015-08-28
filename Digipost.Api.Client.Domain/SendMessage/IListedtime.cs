using System;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IListedtime
    {
        /// <summary>
        ///     Date and Time when the sms will be sent out
        /// </summary>
        DateTime Time { get; set; }

    }
}