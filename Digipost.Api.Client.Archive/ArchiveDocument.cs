using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Relations;

namespace Digipost.Api.Client.Archive
{
    public class ArchiveDocument : RestLinkable, IRequestContent
    {
        internal ArchiveDocument(Guid id, string fileName, string fileType, string contentType)
        {
            Id = id;
            FileName = fileName;
            FileType = fileType;
            ContentType = contentType;
            Attributes = new Dictionary<string, string>();
            Links = new Dictionary<string, Link>();
        }

        public ArchiveDocument(Guid id, string fileName, string fileType, string contentType, byte[] contentBytes)
        {
            Id = id;
            FileName = fileName;
            FileType = fileType;
            ContentType = contentType;
            Attributes = new Dictionary<string, string>();
            Links = new Dictionary<string, Link>();
            ContentBytes = contentBytes;
        }

        public ArchiveDocument WithReferenceId(string referenceId)
        {
            ReferenceId = referenceId;
            return this;
        }

        public ArchiveDocument WithDeletionTime(DateTime deletionTime)
        {
            DeletionTime = deletionTime;
            return this;
        }

        public ArchiveDocument WithAttribute(string key, string value)
        {
            Attributes.Add(key, value);
            return this;
        }

        public ArchiveDocument WithAttributes(Dictionary<string, string> additionalAttributes)
        {
            foreach (var keyValuePair in additionalAttributes)
            {
                Attributes.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return this;
        }

        public Guid Id { get; }

        public string FileName { get; }

        public string FileType { get; }

        public string ContentType { get; }

        public string ReferenceId { get; internal set; }

        public ContentHash ContentHash { get; internal set; }

        public DateTime ArchiveTime { get; internal set; }

        public DateTime DeletionTime { get; internal set; }

        public Dictionary<string, string> Attributes { get; internal set; }

        public byte[] ContentBytes { get; set; }

        public GetArchiveDocumentByUuidUri GetArchiveDocumentByUuidUri()
        {
            return new GetArchiveDocumentByUuidUri(Links["GET_ARCHIVE_DOCUMENT_BY_UUID"]);
        }

        public ArchiveDocumentContentUri DocumentContentUri()
        {
            return new ArchiveDocumentContentUri(Links["GET_ARCHIVE_DOCUMENT_CONTENT"]);
        }

        public ArchiveDocumentContentStreamUri GetDocumentContentStreamUri()
        {
            return new ArchiveDocumentContentStreamUri(Links["GET_ARCHIVE_DOCUMENT_CONTENT_STREAM"]);
        }

        public ArchiveDocumentUpdateUri GetUpdateUri()
        {
            return new ArchiveDocumentUpdateUri(Links["SELF_UPDATE"]);
        }

        public ArchiveDocumentDeleteUri GetDeleteUri()
        {
            return new ArchiveDocumentDeleteUri(Links["SELF_DELETE"]);
        }
    }
}
