using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    //PrintDetails
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "print-details", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class PrintDetails
    {

        private PrintRecipient recipientField;

        private PrintRecipient returnaddressField;

        private PostType posttypeField;

        private PrintColors colorField;

        private NondeliverableHandling nondeliverablehandlingField;

        public PrintDetails()
        {
            this.colorField = PrintColors.MONOCHROME;
            this.nondeliverablehandlingField = NondeliverableHandling.RETURN_TO_SENDER;
        }

        /// <remarks/>
        public PrintRecipient recipient
        {
            get
            {
                return this.recipientField;
            }
            set
            {
                this.recipientField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("return-address")]
        public PrintRecipient returnaddress
        {
            get
            {
                return this.returnaddressField;
            }
            set
            {
                this.returnaddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("post-type")]
        public PostType posttype
        {
            get
            {
                return this.posttypeField;
            }
            set
            {
                this.posttypeField = value;
            }
        }

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(printcolors.MONOCHROME)]
        public PrintColors color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nondeliverable-handling")]
        [System.ComponentModel.DefaultValueAttribute(NondeliverableHandling.RETURN_TO_SENDER)]
        public NondeliverableHandling nondeliverablehandling
        {
            get
            {
                return this.nondeliverablehandlingField;
            }
            set
            {
                this.nondeliverablehandlingField = value;
            }
        }
    }
}
