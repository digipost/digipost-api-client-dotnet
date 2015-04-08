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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "message-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [System.Xml.Serialization.XmlRootAttribute("message-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public partial class MessageRecipient
    {

        private object identificationValue;

        private IdentificationChoice identification;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("digipost-address", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("name-and-address", typeof(NameAndAddress))]
        [System.Xml.Serialization.XmlElementAttribute("organisation-number", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("personal-identification-number", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Identification
        {
            get
            {
                return this.identificationValue;
            }
            set
            {
                this.identificationValue = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IdentificationChoice ItemElementName
        {
            get
            {
                return this.identification;
            }
            set
            {
                this.identification = value;
            }
        }

    }
}
