using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Print
{
    public class PrintDetails : IPrintDetails
    {
        public IPrintRecipient PrintRecipient { get; set; }
        public IPrintReturnRecipient PrintReturnRecipient { get; set; }

        public PrintDetails(IPrintRecipient printRecipient, IPrintReturnRecipient printReturnRecipient, PostType postType = PostType.B, PrintColors printColors = PrintColors.Monochrome)
        {
            PrintRecipient = printRecipient;
            PrintReturnRecipient = printReturnRecipient;
            PostType = postType;
            PrintColors = printColors;
            NondeliverableHandling = NondeliverableHandling.ReturnToSender;
        }

        public PrintRecipient RecipientDataTransferObject { get; set; }

        public PrintReturnRecipient PrintReturnRecipientDataTransferObject { get; set; }
        
        public PostType PostType { get; set; }
        
        public PrintColors PrintColors { get; set; }

        public NondeliverableHandling NondeliverableHandling { get; internal set; }
    }
}
