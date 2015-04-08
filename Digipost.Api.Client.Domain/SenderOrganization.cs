using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sender-organization", Namespace = "http://api.digipost.no/schema/v6")]
    public class SenderOrganization
    {
        private string _organizationId;
        private string _partId;

        /// <remarks />
        [XmlElement("organization-id")]
        public string OrganizationId
        {
            get { return _organizationId; }
            set { _organizationId = value; }
        }

        /// <remarks />
        [XmlElement("part-id")]
        public string PartId
        {
            get { return _partId; }
            set { _partId = value; }
        }
    }
}