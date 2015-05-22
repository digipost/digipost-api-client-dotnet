using System;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute("invoice",Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Invoice : Document
    {

        private Invoice()
        {

        }
        public Invoice(string subject, string mimeType, string path, decimal amount, string account, DateTime duedate, string kid= null,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal) : base(subject,mimeType,path,authLevel,sensitivityLevel)
        {
            Kid = kid;
            Amount = amount;
            Account = account;
            Duedate = duedate;
        }

        public Invoice(string subject, string mimeType, byte[] contentBytes, decimal amount, string account, DateTime duedate, string kid= null,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null): base(subject,mimeType,contentBytes,authLevel,sensitivityLevel)
        {
            Kid = kid;
            Amount = amount;
            Account = account;
            Duedate = duedate;
        }

        [XmlElementAttribute("kid")]
        public string Kid { get; set; }

        [XmlElementAttribute("amount")]
        public decimal Amount { get; set; }

        [XmlElementAttribute("account")]
        public string Account { get; set; }

        [XmlElementAttribute("due-date", DataType = "date")]
        public DateTime Duedate { get; set; }
    }
}
