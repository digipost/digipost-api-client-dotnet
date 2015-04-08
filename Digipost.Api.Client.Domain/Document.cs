using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    // Document
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "document", Namespace = "http://api.digipost.no/schema/v6")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public partial class Document
    {

        private string uuid;

        private string subject;

        private string fileType;

        private AuthenticationLevel authenticationlevel;

        private SensitivityLevel sensitivitylevel;

        private string technicaltypeField;

        private byte[] content;

        /// <remarks/>
        public string Uuid
        {
            get
            {
                return this.uuid;
            }
            set
            {
                this.uuid = value;
            }
        }

        /// <remarks/>
        public string Subject
        {
            get
            {
                return this.subject;
            }
            set
            {
                this.subject = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("file-type")]
        public string FileType
        {
            get
            {
                return this.fileType;
            }
            set
            {
                this.fileType = value;
            }
        }

       

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("authentication-level")]
        public AuthenticationLevel Authenticationlevel
        {
            get
            {
                return this.authenticationlevel;
            }
            set
            {
                this.authenticationlevel = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sensitivity-level")]
        public SensitivityLevel Sensitivitylevel
        {
            get
            {
                return this.sensitivitylevel;
            }
            set
            {
                this.sensitivitylevel = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("technical-type")]
        public string Technicaltype
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
        public byte[] Content
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
