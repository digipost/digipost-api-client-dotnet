using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Send;
using Document = Digipost.Api.Client.Send.Document;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.TestKlient
{
    internal class Program
    {
        // This class is not intended as an example for integrators.
        // Please refer to online documentation at repository root.
        public static void Main(string[] args)
        {
            var environment = Environment.Production;
            environment.Url = new Uri("https://api.qa.digipost.no");

            var config = new ClientConfig(new Broker(1010), environment);
            var client = new DigipostClient(config, thumbprint: "2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed");

            var message = new Message(
                new Sender(10100),
                new RecipientById(IdentificationType.PersonalIdentificationNumber, "01043100358"),
                new Document(subject: "Attachment", fileType: "txt", path: @"\\vmware-host\Shared Folders\Downloads\samlple.xml")
            );

            var result = client.SendMessage(message);
        }
    }
}
