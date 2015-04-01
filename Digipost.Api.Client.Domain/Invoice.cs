using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // Invoice

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://api.digipost.no/schema/v6")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public partial class Invoice : Document
    {

        private string kidField;

        private decimal amountField;

        private string accountField;

        private System.DateTime duedateField;

        /// <remarks/>
        public string kid
        {
            get
            {
                return this.kidField;
            }
            set
            {
                this.kidField = value;
            }
        }

        /// <remarks/>
        public decimal amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string account
        {
            get
            {
                return this.accountField;
            }
            set
            {
                this.accountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("due-date", DataType = "date")]
        public System.DateTime duedate
        {
            get
            {
                return this.duedateField;
            }
            set
            {
                this.duedateField = value;
            }
        }
    }
}
