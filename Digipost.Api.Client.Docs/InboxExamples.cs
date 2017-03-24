using System.Linq;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;

namespace Digipost.Api.Client.Docs
{
    public class InboxExamples
    {
        private static readonly ClientConfig ClientConfig = new ClientConfig(new Broker(123456), Environment.Production);
        private static readonly DigipostClient client = new DigipostClient(ClientConfig, thumbprint: "84e492a972b7e...");
        private static readonly Sender sender = new Sender(67890);

        public void ClientConfiguration()
        {
            // The actual sender of the message. The broker is the owner of the organization certificate 
            // used in the library. The broker id can be retrieved from your Digipost organization account.
            var broker = new Broker(12345);

            // The sender is what the receiver of the message sees as the sender of the message. 
            // If you are delivering on behalf of yourself, set this to your organization`s sender id.
            var sender = new Sender(67890);

            var clientConfig = new ClientConfig(broker, Environment.Production);
            var client = new DigipostClient(clientConfig, thumbprint: "84e492a972b7e...");
        }

        private void Hent_dokumenter()
        {
            var inbox = client.GetInbox(sender);

            var first100 = inbox.Fetch();

            var next200 = inbox.Fetch(offset: 100, limit: 200);
        }

        private async Task Hent_dokumentinnhold()
        {
            var inbox = client.GetInbox(sender);

            var documentMetadata = (await inbox.Fetch()).First();

            var documentStream = await inbox.FetchDocument(documentMetadata);
        }

        private async Task Slett_dokument()
        {
            var inbox = client.GetInbox(sender);

            var documentMetadata = (await inbox.Fetch()).First();

            await inbox.DeleteDocument(documentMetadata);
        }
    }
}