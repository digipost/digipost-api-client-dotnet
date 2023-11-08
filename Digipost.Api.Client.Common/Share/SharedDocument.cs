using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Relations;

namespace Digipost.Api.Client.Common.Share
{
    public class SharedDocument : RestLinkable
    {
        public DateTime DeliveryTime { get; }

        public string Subject { get; }

        public string FileType { get; }

        public int FileSizeBytes { get; }

        public IOrigin Origin { get; }

        public SharedDocument(
            DateTime deliveryTime,
            String subject,
            String fileType,
            int fileSizeBytes,
            IOrigin origin,
            Dictionary<string, Link> links)
            : base(links)
        {
            DeliveryTime = deliveryTime;
            Subject = subject;
            FileType = fileType;
            FileSizeBytes = fileSizeBytes;
            Origin = origin;
        }

        public GetSharedDocumentContentStreamUri GetSharedDocumentContentStreamUri()
        {
            return new GetSharedDocumentContentStreamUri(Links["GET_SHARED_DOCUMENT_CONTENT_STREAM"]);
        }

        public GetSharedDocumentContentUri GetSharedDocumentContentUri()
        {
            return new GetSharedDocumentContentUri(Links["GET_SHARED_DOCUMENT_CONTENT"]);
        }
    }

    public interface IOrigin
    {
        string Name { get; }
    }

    public class OrganisationOrigin : IOrigin
    {
        public string Name { get; }

        public string OrganisationNumber { get; }

        public OrganisationOrigin(string name, string organisationNumber)
        {
            Name = name;
            OrganisationNumber = organisationNumber;
        }

        public override string ToString()
        {
            return Name + "[" + OrganisationNumber + "]";
        }
    }

    public class PrivatePersonOrigin : IOrigin
    {
        public string Name { get; }

        public PrivatePersonOrigin(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
