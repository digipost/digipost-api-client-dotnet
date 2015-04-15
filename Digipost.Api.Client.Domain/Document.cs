using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    // Document
    /// <remarks />
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
        private string _subject;
        private string _technicaltype;
        private string _guid;

        /// <remarks />
        [XmlElement("uuid")]
        public string Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        /// <remarks />
        [XmlElement("subject")]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        /// <remarks />
        [XmlElement("file-type")]
        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        /// <remarks />
        [XmlElement("authentication-level")]
        public AuthenticationLevel Authenticationlevel
        {
            get { return _authenticationlevel; }
            set { _authenticationlevel = value; }
        }

        /// <remarks />
        [XmlElement("sensitivity-level")]
        public SensitivityLevel Sensitivitylevel
        {
            get { return _sensitivitylevel; }
            set { _sensitivitylevel = value; }
        }

        /// <remarks />
        [XmlAttribute("technical-type")]
        public string Technicaltype
        {
            get { return _technicaltype; }
            set { _technicaltype = value; }
        }

        [XmlIgnore]
        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }
    }
}