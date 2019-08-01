namespace Digipost.Api.Client.Common.Print
{
    public class PrintInstruction : IPrintInstruction
    {
        public string key { get; }

        public string value { get; }


        public PrintInstruction(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
