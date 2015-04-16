using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    // Document
    
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "document", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Document
    {
        private AuthenticationLevel _authenticationlevel;
        private byte[] _content;
        private string _fileType;
        private SensitivityLevel _sensitivitylevel;
        private string _technicaltype;

        private Document() { /**Must exist for serialization.**/ }

        public Document(string subject, string filetype, byte[] contentBytes, AuthenticationLevel authLevel = AuthenticationLevel.Password, 
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileType = filetype;
            ContentBytes = contentBytes;
            Authenticationlevel = authLevel;
            Sensitivitylevel = sensitivityLevel;
        }

        [XmlElement("uuid")]
        public string Guid { get; set; }
        
        [XmlElement("subject")]
        public string Subject { get; set; }
        
        [XmlElement("file-type")]
        public string FileType { get; set; }
        
        [XmlElement("authentication-level")]
        public AuthenticationLevel Authenticationlevel { get; set; }

        [XmlElement("sensitivity-level")]
        public SensitivityLevel Sensitivitylevel { get; set; }
        
        [XmlAttribute("technical-type")]
        public string Technicaltype { get; set; }

        [XmlIgnore]
        public byte[] ContentBytes { get; set; }
    }
}