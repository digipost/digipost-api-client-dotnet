using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Testklient
{
    internal class Program
    {
        private const string SenderId = "779052";  //"106768801";
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Api.Client.Testklient.Resources");
        static readonly string Thumbprint = "84e492a972b7edc197a32d9e9c94ea27bd5ac4d9".ToUpper();

        private static void Main(string[] args)
        {
            var message = GetMessage();
            var config = new ClientConfig(SenderId);
            Logging.Initialize(config);

            var api = new DigipostClient(config, Thumbprint);
            var t = api.Send(message);
            
            var r = t.Result;
            Console.ReadKey();
        }

        private static Message GetMessage()
        {
            //primary document
            var doc = new Document("Sensitivt uten bankid", "txt", GetPrimaryDocument());

            //recipient
            var nameandaddr = new NameAndAddress("Kristian Sæther Enge", "Colletts Gate 68", "0460", "Oslo"){
                Email = "kristian.denstore@digipost.no"
            };
            var mr = new MessageRecipient(nameandaddr);

            //message
            var m = new Message(mr, doc);
            return m;
        }

        private static byte[] GetPrimaryDocument()
        {
            return ResourceUtility.ReadAllBytes(true, "Hoveddokument.txt");

        }

        private static byte[] GetAttachment()
        {
            return ResourceUtility.ReadAllBytes(true, "Vedlegg.txt");
        }

    }
}