using System;
using System.Linq;
using Digipost.Api.Client.Common;
using V8 = Digipost.Api.Client.Common.Generated.V8;

namespace Digipost.Api.Client.Archive
{
    internal static class ArchiveDataTransferObjectConverter
    {
        internal static V8.Archive ToDataTransferObject(this Archive a)
        {
            var dto = new V8.Archive()
            {
                Name = a.Name,
                SenderId = a.Sender.Id,
                SenderIdSpecified = true
            };

            foreach (var ad in a.ArchiveDocuments.Select(ToDataTransferObject))
            {
                dto.Documents.Add(ad);
            }

            return dto;
        }

        internal static V8.ArchiveDocument ToDataTransferObject(this ArchiveDocument ad)
        {
            var dto = new V8.ArchiveDocument()
            {
                Uuid = ad.Id.ToString(),
                FileName = ad.FileName,
                FileType = ad.FileType,
                ContentType = ad.ContentType,
                Referenceid = ad.ReferenceId,
                ArchivedTime = ad.ArchiveTime,
                DeletionTime = ad.DeletionTime
            };

            foreach (var attribute in ad.Attributes)
            {
                dto.Attributes.Add(new V8.ArchiveDocumentAttribute(){Key = attribute.Key, Value = attribute.Value});
            }

            if (ad.ContentHash != null)
            {
                dto.ContentHash = new V8.ContentHash()
                {
                    Value = ad.ContentHash.Value,
                    HashAlgorithm = ad.ContentHash.HashAlgoritm
                };
            }

            return dto;
        }

        internal static ArchiveDocument FromDataTransferObject(this V8.ArchiveDocument ad)
        {
            return new ArchiveDocument(
                new Guid(ad.Uuid),
                ad.FileName,
                ad.FileType,
                ad.ContentType
            )
            {
                ReferenceId = ad.Referenceid,
                ContentHash = ad.ContentHash == null ? null : new ContentHash {HashAlgoritm = ad.ContentHash.HashAlgorithm, Value = ad.ContentHash.Value},
                ArchiveTime = ad.ArchivedTime,
                DeletionTime = ad.DeletionTime,
                Attributes = ad.Attributes.ToDictionary(ada => ada.Key, ada => ada.Value),
                Links = ad.Link.FromDataTransferObject()
            };
        }

        internal static Archive FromDataTransferObject(this V8.Archive a)
        {
            return new Archive(new Sender(a.SenderId), a.Name)
            {
                ArchiveDocuments = a.Documents.Select(FromDataTransferObject).ToList(),
                Links = a.Link.FromDataTransferObject()
            };
        }

        internal static ArchiveDocumentContent FromDataTransferObject(this V8.ArchiveDocumentContent result)
        {
            return new ArchiveDocumentContent(result.ContentType, new Uri(result.Uri, UriKind.Absolute));
        }
    }
}
