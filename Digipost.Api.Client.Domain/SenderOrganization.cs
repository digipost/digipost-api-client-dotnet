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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "sender-organization", Namespace = "http://api.digipost.no/schema/v6")]
    public partial class SenderOrganization
    {

        private string organizationidField;

        private string partidField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("organization-id")]
        public string OrganizationId
        {
            get
            {
                return this.organizationidField;
            }
            set
            {
                this.organizationidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("part-id")]
        public string PartId
        {
            get
            {
                return this.partidField;
            }
            set
            {
                this.partidField = value;
            }
        }
    }
}
