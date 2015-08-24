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
        public Invoice(string subject, string fileType, string path, decimal amount, string account, DateTime duedate, string kid= null,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal) : base(subject,fileType,path,authLevel,sensitivityLevel)
        {
            Kid = kid;
            Amount = amount;
            Account = account;
            Duedate = duedate;
        }

        public Invoice(string subject, string fileType, byte[] contentBytes, decimal amount, string account, DateTime duedate, string kid= null,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null): base(subject,fileType,contentBytes,authLevel,sensitivityLevel)
        {
            Kid = kid;
            Amount = amount;
            Account = account;
            Duedate = duedate;
        }

        /// <summary>
        /// Customer identification number. 2 to 25 digits with no spaces or dots. Mandatory by default.  
        /// </summary>
        [XmlElementAttribute("kid")]
        public string Kid { get; set; }

        /// <summary>
        /// The amount of the invoice.
        /// </summary>
        [XmlElementAttribute("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Receiving account. 11 digits with no spaces or dots.
        /// </summary>
        [XmlElementAttribute("account")]
        public string Account { get; set; }
        
        /// <summary>
        /// When the invoice is due.
        /// </summary>
        [XmlElementAttribute("due-date", DataType = "date")]
        public DateTime Duedate { get; set; }
    }
}
