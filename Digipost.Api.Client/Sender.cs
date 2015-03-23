using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client
{

    public class Sender
    {

        public static readonly string BaseAddress = "https://qa2.api.digipost.no/";

        public static async Task<string> SendAsync(string forsendelseId, string digipostAdresse, string emne)
        {
            var loggingHandler = new LoggingHandler(new HttpClientHandler());

            using (var client = new HttpClient(loggingHandler))
            {
                client.BaseAddress = new Uri(BaseAddress);
                
                client.DefaultRequestHeaders.Add("X-Digipost-UserId", GetUserId());
                client.DefaultRequestHeaders.Add("Date", DateTime.UtcNow.ToString("R"));
                client.DefaultRequestHeaders.Add("X-Content-SHA256", ComputeHash(GetXmlMessage()));

                var request = new HttpRequestMessage(HttpMethod.Post, "messages");
                request.Content = new StringContent(GetXmlMessage(), Encoding.UTF8, "application/vnd.digipost-v6+xml");
                var response = await client.SendAsync(request);
                
            }

            return null;
        }

        private static string ComputeHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

            return Convert.ToBase64String(hashedBytes);
        }

        private static string GetXmlMessage()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><message xmlns=\"http://api.digipost.no/schema/v6\"><recipient><personal-identification-number>01013300001</personal-identification-number></recipient><primary-document><uuid>37740f5c-3654-45b8-923f-be9fc8a56af5</uuid><subject>Dokumentets emne</subject><file-type>pdf</file-type><sms-notification><after-hours>1</after-hours></sms-notification><authentication-level>PASSWORD</authentication-level><sensitivity-level>NORMAL</sensitivity-level></primary-document></message>";
        }

        private static string GetUserId()
        {
            return "106768801";
        }
    }
}
