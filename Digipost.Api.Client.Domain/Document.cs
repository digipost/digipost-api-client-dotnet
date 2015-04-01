using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // Document
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Invoice))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "document", Namespace = "http://api.digipost.no/schema/v6")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public partial class Document
    {

        private string uuidField;

        private string subjectField;

        private string filetypeField;

        private object itemField;

        private SmsNotification smsnotificationField;

        private EmailNotification emailnotificationField;

        private AuthenticationLevel authenticationlevelField;

        private bool authenticationlevelFieldSpecified;

        private SensitivityLevel sensitivitylevelField;

        private bool sensitivitylevelFieldSpecified;

        private bool preencryptField;

        private bool preencryptFieldSpecified;

        private ContentHash contenthashField;

        private Link[] linkField;

        private string technicaltypeField;

        private byte[] content;

        /// <remarks/>
        public string uuid
        {
            get
            {
                return this.uuidField;
            }
            set
            {
                this.uuidField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("file-type")]
        public string filetype
        {
            get
            {
                return this.filetypeField;
            }
            set
            {
                this.filetypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("opened", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("opening-receipt", typeof(string))]
        public object Item
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
        [System.Xml.Serialization.XmlElementAttribute("sms-notification")]
        public SmsNotification smsnotification
        {
            get
            {
                return this.smsnotificationField;
            }
            set
            {
                this.smsnotificationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("email-notification")]
        public EmailNotification emailnotification
        {
            get
            {
                return this.emailnotificationField;
            }
            set
            {
                this.emailnotificationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("authentication-level")]
        public AuthenticationLevel authenticationlevel
        {
            get
            {
                return this.authenticationlevelField;
            }
            set
            {
                this.authenticationlevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool authenticationlevelSpecified
        {
            get
            {
                return this.authenticationlevelFieldSpecified;
            }
            set
            {
                this.authenticationlevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sensitivity-level")]
        public SensitivityLevel sensitivitylevel
        {
            get
            {
                return this.sensitivitylevelField;
            }
            set
            {
                this.sensitivitylevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sensitivitylevelSpecified
        {
            get
            {
                return this.sensitivitylevelFieldSpecified;
            }
            set
            {
                this.sensitivitylevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pre-encrypt")]
        public bool preencrypt
        {
            get
            {
                return this.preencryptField;
            }
            set
            {
                this.preencryptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool preencryptSpecified
        {
            get
            {
                return this.preencryptFieldSpecified;
            }
            set
            {
                this.preencryptFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("content-hash")]
        public ContentHash contenthash
        {
            get
            {
                return this.contenthashField;
            }
            set
            {
                this.contenthashField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("link")]
        public Link[] link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("technical-type")]
        public string technicaltype
        {
            get
            {
                return this.technicaltypeField;
            }
            set
            {
                this.technicaltypeField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnore]
        public byte[] contentOfDocument
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }
    }
}
