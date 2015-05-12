using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace Digipost.Api.Client
{

    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "identification-result", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class IdentificationResult
    {
        [XmlElement("result")]
        public IdentificationResultCode IdentificationResultCode { get; set; }

        public string digipostAddress { get; set; }

   

    }
}
