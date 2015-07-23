using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <summary>
    /// Used when sending messages on behalf of an organization if 'using external sender id'
    /// is activated. If activated in Digipost admin (can only be done by Digipost Support), 
    /// the organization can be identified by organization number, or organisation number
    /// and unit id.  
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sender-organization", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class SenderOrganization
    {
        private SenderOrganization()
        {
            /**Must exist for serialization.**/
        }

        /// <summary>
        /// The sender organization can be identified by an organization number and a unit id 
        /// if activated in Digipost admin (can onle be done by Digipost Support). 
        /// </summary>
        /// <param name="organizationNumber">Organization number for the sender organization.</param>
        /// <param name="unitId">The unit within the organization.</param>
        public SenderOrganization(string organizationNumber, string unitId = "")
        {
            OrganizationNumber = organizationNumber;
            UnitId = unitId;
        }

        /// <summary>
        /// Organization number for the sender organization.
        /// </summary>
        [XmlElement("organization-id")]
        public string OrganizationNumber { get; set; }

        /// <summary>
        /// The unit within the organization.
        /// </summary>
        [XmlElement("part-id")]
        public string UnitId { get; set; }
    }
}
