namespace Digipost.Api.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = Sender.SendAsync("forsendelseId", "Digipostadresse", "Emne");
            var r = t.Result;
        }
    }
}


