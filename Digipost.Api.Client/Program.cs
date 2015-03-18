using System.Reflection;

namespace Digipost.Api.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
