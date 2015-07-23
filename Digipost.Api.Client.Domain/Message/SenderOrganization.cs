using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{

    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sender-organization", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class SenderOrganization
    {
        [XmlElement("organization-id")]
        public string OrganizationId { get; set; }

        [XmlElement("part-id")]
        public string PartId { get; set; }
    }
}
