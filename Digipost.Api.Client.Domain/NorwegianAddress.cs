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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "norwegian-address", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class NorwegianAddress
    {

        private string addressline1Field;

        private string addressline2Field;

        private string addressline3Field;

        private string zipcodeField;

        private string cityField;

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
        [System.Xml.Serialization.XmlElementAttribute("zip-code")]
        public string zipcode
        {
            get
            {
                return this.zipcodeField;
            }
            set
            {
                this.zipcodeField = value;
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
    }
}
