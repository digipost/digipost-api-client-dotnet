using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [XmlType(TypeName = "sensitivity-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum SensitivityLevel
    {
        [XmlEnum("NORMAL")] 
        Normal,

        [XmlEnum("SENSITIVE")] 
        Sensitive
    }
}