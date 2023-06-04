using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Print;

namespace Digipost.Api.Client.Send
{
    public class PrintMessage : Message
    {
        public PrintMessage(Sender sender, IPrintDetails printDetails, IDocument primaryDocument)
            : base(sender, null, primaryDocument)
        {
            PrintDetails = printDetails;
        }

        public override string ToString()
        {
            return PrintDetails.ToString();
        }
    }
}
