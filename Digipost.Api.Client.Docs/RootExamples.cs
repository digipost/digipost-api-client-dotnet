using System.ComponentModel.Design;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.DataTypes.Core;
using Sender = Digipost.Api.Client.Common.Sender;

#pragma warning disable 0169
#pragma warning disable 0649

namespace Digipost.Api.Client.Docs
{
    public class RootExamples
    {
        private static readonly DigipostClient client;
        private static readonly Sender sender;

        private void FetchDefaultRoot()
        {
            var root = client.GetRoot(new ApiRootUri());
        }

        private void FetchSenderRoot()
        {
            var root = client.GetRoot(new ApiRootUri(new Sender(1234)));
        }
    }
}
