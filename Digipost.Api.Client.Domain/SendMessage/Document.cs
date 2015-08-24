using System;
using System.IO;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Document : IDocument
    {
        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="path">The path to the file. e.g c:\docs\file01.txt</param>
        /// <param name="authenticationLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        public Document(string subject, string fileType, string path,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileType = fileType;
            FileType = fileType;
            ContentBytes = ReadAllBytes(path);
            AuthenticationLevel = authenticationLevel;
            SensitivityLevel = sensitivityLevel;
        }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="contentBytes">The content of file in byteArray.</param>
        /// <param name="authenticationLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        /// <param name="smsNotification">Sets SMS notification setting. Default null.</param>
        public Document(string subject, string fileType, byte[] contentBytes,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileType = fileType;
            ContentBytes = contentBytes;
            AuthenticationLevel = authenticationLevel;
            SensitivityLevel = sensitivityLevel;
            SmsNotification = smsNotification;
        }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The type of the file. e.g pdf,txt..</param>
        /// <param name="documentStream">Stream of the file.</param>
        /// <param name="authenticationLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        /// <param name="smsNotification">Sets SMS notification setting. Default null.</param>
        public Document(string subject, string fileType, Stream documentStream,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, SmsNotification smsNotification = null)
        {
            Guid = System.Guid.NewGuid().ToString();
            Subject = subject;
            FileType = fileType;
            FileType = fileType;
            ContentBytes = File.ReadAllBytes(new StreamReader(documentStream).ReadToEnd());
            AuthenticationLevel = authenticationLevel;
            SensitivityLevel = sensitivityLevel;
            SmsNotification = smsNotification;
        }

        public string Guid { get; set; }
        
        public string Subject { get; set; }

        [Obsolete("Deprecated, use FileType instead. NB. This will be removed in future version.")]
        public string MimeType
        {
            get { return FileType; }
            set { FileType = value; }
        }

        public string FileType { get; set; }
        
        public SmsNotification SmsNotification { get; set; }
        
        public AuthenticationLevel AuthenticationLevel { get; set; }
        
        public SensitivityLevel SensitivityLevel { get; set; }
        
        public byte[] ContentBytes { get; set; }

        private byte[] ReadAllBytes(string pathToDocument)
        {
            return File.ReadAllBytes(pathToDocument);
        }
    }
}
