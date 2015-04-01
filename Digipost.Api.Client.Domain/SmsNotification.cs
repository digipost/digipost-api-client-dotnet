using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // SmsNotification

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class SmsNotification
    {

        private SmsOverrides overridesField;

        private ListedTime[] atField;

        private int[] afterhoursField;

        /// <remarks/>
        public SmsOverrides overrides
        {
            get
            {
                return this.overridesField;
            }
            set
            {
                this.overridesField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("after-hours")]
        public int[] afterhours
        {
            get
            {
                return this.afterhoursField;
            }
            set
            {
                this.afterhoursField = value;
            }
        }
    }
}
