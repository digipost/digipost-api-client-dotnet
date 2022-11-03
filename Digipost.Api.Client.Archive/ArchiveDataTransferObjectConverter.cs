using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using V8;
using Link = Digipost.Api.Client.Common.Entrypoint.Link;

namespace Digipost.Api.Client.Archive
{
    internal static class ArchiveDataTransferObjectConverter
    {
        public static Archive_Document ToDataTransferObject(ArchiveDocument ad)
        {
            var dto = new Archive_Document()
            {
                Uuid = ad.Id.ToString(),
                File_Name = ad.FileName,
                File_Type = ad.FileType,
                Content_Type = ad.ContentType,
                Referenceid = ad.ReferenceId,
                Archived_Time = ad.ArchiveTime,
                Deletion_Time = ad.DeletionTime
            };

            if (ad.ContentHash != null)
            {
                dto.Content_Hash = new Content_Hash()
                {
                    Value = ad.ContentHash.Value,
                    Hash_Algorithm = ad.ContentHash.HashAlgoritm
                };
            }

            return dto;
        }

        public static Dictionary<string, Link> FromDataTransferObject(IEnumerable<V8.Link> links)
        {
            return links.ToDictionary(
                l => l.Rel.Substring(l.Rel.LastIndexOf('/')+1).ToUpper(),
                link => new Link(link.Uri) {Rel = link.Rel, MediaType = link.Media_Type}
            );
        }

        public static ArchiveDocument FromDataTransferObject(Archive_Document ad)
        {
            return new ArchiveDocument(
                new Guid(ad.Uuid),
                ad.File_Name,
                ad.File_Type,
                ad.Content_Type
            )
            {
                ReferenceId = ad.Referenceid,
                ContentHash = ad.Content_Hash == null ? null : new ContentHash {HashAlgoritm = ad.Content_Hash.Hash_Algorithm, Value = ad.Content_Hash.Value},
                ArchiveTime = ad.Archived_Time,
                DeletionTime = ad.Deletion_Time,
                Attributes = ad.Attributes.ToDictionary(ada => ada.Key, ada => ada.Value),
                Links = FromDataTransferObject(ad.Link)
            };
        }

        public static Archive FromDataTransferObject(V8.Archive a)
        {
            return new Archive(a.Name, new Sender(a.Sender_Id))
            {
                ArchiveDocuments = a.Documents.Select(FromDataTransferObject).ToList(),
                Links = FromDataTransferObject(a.Link)
            };
        }
    }
}
