using System;
using System.IO;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Archive
{
    internal interface IArchive
    {
        /**
         * This will hash and create a Guid the same way as java UUID.nameUUIDFromBytes
         */
        Task<Stream> StreamDocumentFromExternalId(String externalId);
        Task<Stream> StreamDocumentFromExternalId(Guid externalId_guid);

    }
}
