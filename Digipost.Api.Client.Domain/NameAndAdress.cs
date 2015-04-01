using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "name-and-address", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class NameAndAddress
    {

        private string fullnameField;

        private string addressline1Field;

        private string addressline2Field;

        private string postalcodeField;

        private string cityField;

        private System.DateTime birthdateField;

        private bool birthdateFieldSpecified;

        private string phonenumberField;

        private string emailaddressField;

        /// <remarks/>
        public string fullname
        {
            get
            {
                return this.fullnameField;
            }
            set
            {
                this.fullnameField = value;
            }
        }

        /// <remarks/>
        public string addressline1
        {
            get
            {
                return this.addressline1Field;
            }
            set
            {
                this.addressline1Field = value;
            }
        }

        /// <remarks/>
        public string addressline2
        {
            get
            {
                return this.addressline2Field;
            }
            set
            {
                this.addressline2Field = value;
            }
        }

        /// <remarks/>
        public string postalcode
        {
            get
            {
                return this.postalcodeField;
            }
            set
            {
                this.postalcodeField = value;
            }
        }

        /// <remarks/>
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("birth-date", DataType = "date")]
        public System.DateTime birthdate
        {
            get
            {
                return this.birthdateField;
            }
            set
            {
                this.birthdateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool birthdateSpecified
        {
            get
            {
                return this.birthdateFieldSpecified;
            }
            set
            {
                this.birthdateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("phone-number")]
        public string phonenumber
        {
            get
            {
                return this.phonenumberField;
            }
            set
            {
                this.phonenumberField = value;
            }
        }

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
    }
}
