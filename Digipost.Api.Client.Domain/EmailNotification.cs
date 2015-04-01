using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // EmailNotification
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "email-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class EmailNotification
    {

        private string emailaddressField;

        private string subjectField;

        private string textField;

        private ListedTime[] atField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("email-address")]
        public string emailaddress
        {
            get
            {
                return this.emailaddressField;
            }
            set
            {
                this.emailaddressField = value;
            }
        }

        /// <remarks/>
        public string subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("at")]
        public ListedTime[] at
        {
            get
            {
                return this.atField;
            }
            set
            {
                this.atField = value;
            }
        }
    }
}
