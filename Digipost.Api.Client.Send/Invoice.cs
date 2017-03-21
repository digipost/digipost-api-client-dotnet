using System;
using System.IO;
using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Send
{
    public class Invoice : Document, IInvoice
    {
        public Invoice(string subject, string fileType, byte[] contentBytes, decimal amount, string account, DateTime duedate, string kid = null,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : base(subject, fileType, contentBytes, authenticationLevel, sensitivityLevel, smsNotification)
        {
            Kid = kid;
            Amount = amount;
            Account = account;
            Duedate = duedate;
        }

        public Invoice(string subject, string fileType, string path, decimal amount, string account, DateTime duedate, string kid = null,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : this(
                subject, fileType, ExtractBytesFromPath(path), amount, account, duedate, kid, authenticationLevel,
                sensitivityLevel, smsNotification)
        {
        }

        public Invoice(string subject, string fileType, Stream contentStream, decimal amount, string account,
            DateTime duedate, string kid = null,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : this(
                subject, fileType, ExtractBytesFromStream(contentStream), amount, account, duedate, kid,
                authenticationLevel, sensitivityLevel, smsNotification)
        {
        }

        public string Kid { get; set; }

        public decimal Amount { get; set; }

        public string Account { get; set; }

        public DateTime Duedate { get; set; }

        private static byte[] ExtractBytesFromPath(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                return ExtractBytesFromStream(fileStream);
            }
        }

        private static byte[] ExtractBytesFromStream(Stream fileStream)
        {
            if (fileStream == null) return null;
            var bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}