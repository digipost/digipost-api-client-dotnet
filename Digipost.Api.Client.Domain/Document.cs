using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "document", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Document
    {
        private Document()
        {
            /**Must exist for serialization.**/
        }

        public Document(string subject, string filetype, byte[] contentBytes,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileType = filetype;
            ContentBytes = contentBytes;
            Authenticationlevel = authLevel;
            Sensitivitylevel = sensitivityLevel;
            SmsNotification = smsNotification;
        }

        /// <summary>
        ///     Unique identification of document. Is set automatically using System.Guid.NewGuid(), and is
        ///     necessary to change.
        /// </summary>
        [XmlElement("uuid")]
        public string Guid { get; set; }

        /// <summary>
        ///     The subject of the message
        /// </summary>
        [XmlElement("subject")]
        public string Subject { get; set; }

        /// <summary>
        ///     The file type of the document, indicated by MIME-type.
        /// </summary>
        [XmlElement("file-type")]
        public string FileType { get; set; }

        /// <summary>
        ///     Optional SMS notification for the document
        /// </summary>
        [XmlElement("sms-notification")]
        public SmsNotification SmsNotification { get; set; }

        /// <summary>
        ///     The level of authentication for the document.
        /// </summary>
        [XmlElement("authentication-level")]
        public AuthenticationLevel Authenticationlevel { get; set; }

        /// <summary>
        ///     The sensitivity level for the document.
        /// </summary>
        [XmlElement("sensitivity-level")]
        public SensitivityLevel Sensitivitylevel { get; set; }

        /// <summary>
        ///     The technical type of the document.
        /// </summary>
        [XmlAttribute("technical-type")]
        public string Technicaltype { get; set; }

        /// <summary>
        ///     The document encoded as a byte array.
        /// </summary>
        [XmlIgnore]
        public byte[] ContentBytes { get; set; }
    }
}