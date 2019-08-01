using System.Collections.Generic;

namespace Digipost.Api.Client.Common.Print
{
    public class PrintInstructions : IPrintInstructions
    {
        public List<PrintInstruction> PrintInstruction { get; set; }
        
        public PrintInstructions() {}

        public PrintInstructions(List<PrintInstruction> printInstruction)
        {
            this.PrintInstruction = printInstruction;
        }
    }
}
