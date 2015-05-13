using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileMimeType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="pathToDocument">The path to the file. e.g c:\docs\file01.txt</param>
        /// <param name="authLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        public Document(string subject, string fileMimeType, string pathToDocument,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileMimeType = fileMimeType;
            ContentBytes = ReadAllBytes(pathToDocument);
            AuthenticationLevel = authLevel;
            SensitivityLevel = sensitivityLevel;
        }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileMimeType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="contentBytes">The content of file in byteArray.</param>
        /// <param name="authLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        /// <param name="smsNotification">Sets SMS notification setting. Default null.</param>
        public Document(string subject, string fileMimeType, byte[] contentBytes,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileMimeType = fileMimeType;
            ContentBytes = contentBytes;
            AuthenticationLevel = authLevel;
            SensitivityLevel = sensitivityLevel;
            SmsNotification = smsNotification;
        }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileMimeType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="documentStream">Stream of the file.</param>
        /// <param name="authLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        /// <param name="smsNotification">Sets SMS notification setting. Default null.</param>
        public Document(string subject, string fileMimeType, Stream documentStream,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileMimeType = fileMimeType;
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
        [XmlElement("file-type")]
        public string FileMimeType { get; set; }

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
                "Guid: {0}, Subject: {1}, FileMimeType: {2}, SmsNotification: {3}, AuthenticationLevel: {4}, " +
                "SensitivityLevel: {5}, TechnicalType: {6}",
                Guid, Subject, FileMimeType, SmsNotification, AuthenticationLevel, SensitivityLevel, TechnicalType);
        }
    }
}