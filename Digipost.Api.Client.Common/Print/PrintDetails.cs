using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Print
{
    public class PrintDetails : IPrintDetails
    {
        public PrintDetails(IPrintRecipient printRecipient, IPrintReturnRecipient printReturnRecipient, PostType postType = PostType.B, PrintColors printColors = PrintColors.Monochrome)
        {
            PrintRecipient = printRecipient;
            PrintReturnRecipient = printReturnRecipient;
            PostType = postType;
            PrintColors = printColors;
            NondeliverableHandling = NondeliverableHandling.ReturnToSender;
        }

        public IPrintRecipient PrintRecipient { get; set; }

        public IPrintReturnRecipient PrintReturnRecipient { get; set; }

        public PostType PostType { get; set; }

        public PrintColors PrintColors { get; set; }

        public NondeliverableHandling NondeliverableHandling { get; internal set; }
    }
}