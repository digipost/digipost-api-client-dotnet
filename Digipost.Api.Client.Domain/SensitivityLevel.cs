using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [XmlType(TypeName = "sensitivity-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum SensitivityLevel
    {
        /// <remarks />
        [XmlEnum("NORMAL")] Normal,

        /// <remarks />
        [XmlEnum("SENSITIVE")] Sensitive
    }
}