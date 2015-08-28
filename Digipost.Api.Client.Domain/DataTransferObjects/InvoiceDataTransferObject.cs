using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType("invoice",Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class InvoiceDataTransferObject : DocumentDataTransferObject
    {
      public InvoiceDataTransferObject(string subject, string fileType, byte[] contentBytes, decimal amount, string account, DateTime duedate, string kid= null,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null): base(subject,fileType,contentBytes,authLevel,sensitivityLevel)
        {
            Kid = kid;
            Amount = amount;
            Account = account;
            Duedate = duedate;
        }

        internal InvoiceDataTransferObject()
        {
            /* Must exist for serialization */
        }

        /// <summary>
        /// Customer identification number. 2 to 25 digits with no spaces or dots. Mandatory by default.  
        /// </summary>
        [XmlElement("kid")]
        public string Kid { get; set; }

        /// <summary>
        /// The amount of the invoice.
        /// </summary>
        [XmlElement("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Receiving account. 11 digits with no spaces or dots.
        /// </summary>
        [XmlElement("account")]
        public string Account { get; set; }
        
        /// <summary>
        /// When the invoice is due.
        /// </summary>
        [XmlElement("due-date", DataType = "date")]
        public DateTime Duedate { get; set; }
    }
}
