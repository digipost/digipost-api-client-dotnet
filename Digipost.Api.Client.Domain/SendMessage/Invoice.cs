using System;
using System.IO;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Invoice : Document, IInvoice
    {
        public string Kid { get; set; }
        public decimal Amount { get; set; }
        public string Account { get; set; }
        public DateTime Duedate { get; set; }

        public Invoice(string subject, string fileType, byte[] contentBytes, decimal amount, string account, DateTime duedate, string kid= null,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
            : base(subject,fileType,contentBytes,authenticationLevel,sensitivityLevel, smsNotification)
        {
            Kid = kid;
            Amount = amount;
            Account = account;
            Duedate = duedate;
        }

        public Invoice(string subject, string fileType, string path, decimal amount, string account, DateTime duedate, string kid = null,
           AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
           SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
            : this(subject,fileType, new byte[]{}, amount, account, duedate, kid, authenticationLevel, sensitivityLevel, smsNotification)
        {
            
        }

        public Invoice(string subject, string fileType, Stream contentStream, decimal amount, string account, DateTime duedate, string kid = null,
           AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
           SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
            : this(subject, fileType, new byte[] { }, amount, account, duedate, kid, authenticationLevel, sensitivityLevel, smsNotification)
        {

        }
    }
}
