using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // SmsOverrides
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "sms-overrides", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class SmsOverrides
    {

        private string smsmobilenumberField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sms-mobile-number")]
        public string smsmobilenumber
        {
            get
            {
                return this.smsmobilenumberField;
            }
            set
            {
                this.smsmobilenumberField = value;
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
    }
}
