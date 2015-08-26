using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain
{
    [XmlIncludeAttribute(typeof(InvoiceDataTransferObject))]
    [SerializableAttribute]
    [DebuggerStepThrough]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute("document", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRootAttribute(Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public class DocumentDataTransferObject : IDocument
    {
        public DocumentDataTransferObject(string subject, string fileType, byte[] contentBytes,
            AuthenticationLevel authLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileType = fileType;
            ContentBytes = contentBytes;
            AuthenticationLevel = authLevel;
            SensitivityLevel = sensitivityLevel;
            SmsNotification = smsNotification;
        }
        
        [XmlElement("uuid")]
        public string Guid { get; set; }

        [XmlElement("subject")]
        public string Subject { get; set; }

        [XmlElement("file-type")]
        public string FileType { get; set; }

        [XmlElement("sms-notification")]
        public SmsNotification SmsNotification { get; set; }

        [XmlElement("authentication-level")]
        public AuthenticationLevel AuthenticationLevel { get; set; }
        
        [XmlElement("sensitivity-level")]
        public SensitivityLevel SensitivityLevel { get; set; }
        
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
                "SensitivityLevel: {5}",
                Guid, Subject, FileType, SmsNotification, AuthenticationLevel, SensitivityLevel);
        }
    }
}