using System;
using System.IO;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Archive
{
    internal interface IArchive
    {
        Task<Stream> FetchDocument(String externalId);

    }
}
