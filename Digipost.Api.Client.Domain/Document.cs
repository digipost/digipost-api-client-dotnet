using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [XmlIncludeAttribute(typeof(Invoice))]
    [SerializableAttribute()]
    [DebuggerStepThrough]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute("document", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class Document
    {

        protected Document() { }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="path">The path to the file. e.g c:\docs\file01.txt</param>
        /// <param name="authLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        public Document(string subject, string fileType, string path,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            MimeType = fileType;
            FileType = fileType;
            ContentBytes = ReadAllBytes(path);
            AuthenticationLevel = authLevel;
            SensitivityLevel = sensitivityLevel;
        }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="contentBytes">The content of file in byteArray.</param>
        /// <param name="authLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        /// <param name="smsNotification">Sets SMS notification setting. Default null.</param>
        public Document(string subject, string fileType, byte[] contentBytes,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            this.MimeType = fileType;
            ContentBytes = contentBytes;
            AuthenticationLevel = authLevel;
            SensitivityLevel = sensitivityLevel;
            SmsNotification = smsNotification;
        }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The type of the file. e.g pdf,txt..</param>
        /// <param name="documentStream">Stream of the file.</param>
        /// <param name="authLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        /// <param name="smsNotification">Sets SMS notification setting. Default null.</param>
        public Document(string subject, string fileType, Stream documentStream,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileType = fileType;
            MimeType = fileType;
            ContentBytes = File.ReadAllBytes(new StreamReader(documentStream).ReadToEnd());
            AuthenticationLevel = authLevel;
            SensitivityLevel = sensitivityLevel;
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
        [Obsolete("Deprecated, use FileType instead. NB. This will be removed in future version.")]
        public string MimeType
        {
            get { return FileType; }
            set { FileType = value; }
        }

        /// <summary>
        ///     The file type of the document, indicated by MIME-type.
        /// </summary>
        [XmlElement("file-type")]
        public string FileType { get; set; }

        /// <summary>
        ///     Optional SMS notification to Recipient.
        ///     Additional charges apply.
        /// </summary>
        [XmlElement("sms-notification")]
        public SmsNotification SmsNotification { get; set; }

        /// <summary>
        ///     The level of authentication for the document.
        /// </summary>
        [XmlElement("authentication-level")]
        public AuthenticationLevel AuthenticationLevel { get; set; }

        /// <summary>
        ///     Sets the sensitivity level for the document.
        /// </summary>
        [XmlElement("sensitivity-level")]
        public SensitivityLevel SensitivityLevel { get; set; }

        /// <summary>
        ///     This attribute is for Digipost internal-use.
        /// </summary>
        /// This field should not be exposed to customers/senders. It is used to make documents invisible in the inbox. (technical documents)
        [XmlAttribute("technical-type")]
        private string TechnicalType { get; set; }

        /// <summary>
        ///     The document encoded as a byte array.
        /// </summary>
        [XmlIgnore]
        public byte[] ContentBytes { get; set; }

        private byte[] ReadAllBytes(string pathToDocument)
        {
            return File.ReadAllBytes(pathToDocument);
        }

        public override string ToString()
        {
            return string.Format(
                "Guid: {0}, Subject: {1}, mimeType: {2}, SmsNotification: {3}, AuthenticationLevel: {4}, " +
                "SensitivityLevel: {5}, TechnicalType: {6}",
                Guid, Subject, MimeType, SmsNotification, AuthenticationLevel, SensitivityLevel, TechnicalType);
        }
    }
}