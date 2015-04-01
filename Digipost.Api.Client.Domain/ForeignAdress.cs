using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // ForeignAdress
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "foreign-address", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class ForeignAdress
    {

        private string addressline1Field;

        private string addressline2Field;

        private string addressline3Field;

        private string addressline4Field;

        private string itemField;

        private Country itemElementNameField;

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
        public string addressline3
        {
            get
            {
                return this.addressline3Field;
            }
            set
            {
                this.addressline3Field = value;
            }
        }

        /// <remarks/>
        public string addressline4
        {
            get
            {
                return this.addressline4Field;
            }
            set
            {
                this.addressline4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("country", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("country-code", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public Country ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }
}
