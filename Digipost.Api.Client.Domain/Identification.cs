using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "identification", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Identification
    {

        [XmlElement("personal-identification-number")]
        public string PersonalIdentifcationNumber { get; set; }

        [XmlElement("name-and-address")]
        public RecipientByNameAndAddress RecipientByNameAndAddress { get; set; }

    }
}
