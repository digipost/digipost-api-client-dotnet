using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common.Print
{
    public class PrintDetails : IPrintDetails
    {
        public PrintDetails(IPrintRecipient printRecipient, IPrintReturnRecipient printReturnRecipient, PrintColors printColors = PrintColors.Monochrome)
        {
            PrintRecipient = printRecipient;
            PrintReturnRecipient = printReturnRecipient;
            PrintColors = printColors;
            NondeliverableHandling = NondeliverableHandling.ReturnToSender;
        }

        public IPrintRecipient PrintRecipient { get; set; }

        public IPrintReturnRecipient PrintReturnRecipient { get; set; }

        public PrintColors PrintColors { get; set; }

        public NondeliverableHandling NondeliverableHandling { get; internal set; }
    }
}
