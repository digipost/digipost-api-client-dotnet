using System.Threading.Tasks;
using Digipost.Api.Client.Common;
#pragma warning disable 0169
#pragma warning disable 0649

namespace Digipost.Api.Client.Docs
{
    public class ArchiveExamples
    {
        private static readonly DigipostClient client;
        private static readonly Sender sender;

        private async Task Hent_arkivdokument()
        {
            var streamDocumentFromExternalId = await client.GetArchive(sender).StreamDocumentFromExternalId("My uniq referance");
        }

    }
}
