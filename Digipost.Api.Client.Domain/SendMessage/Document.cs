using System;
using System.IO;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public class Document : IDocument
    {
        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="contentBytes">The content of file in byteArray.</param>
        /// <param name="authenticationLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        /// <param name="smsNotification">Sets SMS notification setting. Default null.</param>
        public Document(string subject, string fileType, byte[] contentBytes,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
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
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : this(subject, fileType, new byte[] {}, authenticationLevel, sensitivityLevel, smsNotification)
        {
            ContentBytes = ReadAllBytes(documentStream);
        }

        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="path">The path to the file. e.g c:\docs\file01.txt</param>
        /// <param name="authenticationLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        public Document(string subject, string fileType, string path,
            AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : this(subject, fileType, new byte[] {}, authenticationLevel, sensitivityLevel, smsNotification)
        {
            ContentBytes = ReadAllBytes(path);
        }


        /// <param name="subject">The subject of the document.</param>
        /// <param name="fileType">The mime type of the file. e.g pdf,txt..</param>
        /// <param name="authenticationLevel">Required authentication level of the document. Default password.</param>
        /// <param name="sensitivityLevel">Sensitivity level of the document. Default normal.</param>
        internal Document(string subject, string fileType, AuthenticationLevel authenticationLevel = AuthenticationLevel.Password,
            SensitivityLevel sensitivityLevel = SensitivityLevel.Normal, ISmsNotification smsNotification = null)
            : this(subject, fileType, new byte[] { }, authenticationLevel, sensitivityLevel, smsNotification)
        {
        }

        public string Guid { get; set; }

        public string Subject { get; set; }

        public string FileType { get; set; }

        public ContentHash ContentHash { get; internal set; }

        public ISmsNotification SmsNotification { get; set; }

        public AuthenticationLevel AuthenticationLevel { get; set; }

        public SensitivityLevel SensitivityLevel { get; set; }

        public byte[] ContentBytes { get; set; }

        internal virtual byte[] ReadAllBytes(string pathToDocument)
        {
            return File.ReadAllBytes(pathToDocument);
        }

        internal virtual byte[] ReadAllBytes(Stream documentStream)
        {
            return File.ReadAllBytes(new StreamReader(documentStream).ReadToEnd());
        }
    }
}