using System.Collections.Generic;

namespace Digipost.Api.Client.Common.Print
{
    public interface IPrintInstructions
    {
        List<PrintInstruction> PrintInstruction { get; set; }
    }
}
