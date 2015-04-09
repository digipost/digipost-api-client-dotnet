using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Digipost.Api.Client.Digipost.Api.Client;
using Digipost.Api.Client.Domain;
using DigipostApiClientShared;
using DigipostApiClientShared.Enums;

namespace Digipost.Api.Client
{
    public class DigipostApi
    {
        private ClientConfig ClientConfig { get; set; }

        public DigipostApi(ClientConfig clientConfig)
        {
            ClientConfig = clientConfig;
        }

        public async Task<string> Send(Message message)
        {
            const string uri = "messages";
            Logging.Log(TraceEventType.Information, "> Starting Send()");
            var loggingHandler = new LoggingHandler(new HttpClientHandler());
            var authenticationHandler = new AuthenticationHandler(ClientConfig, uri, loggingHandler);
            Logging.Log(TraceEventType.Information, " - Initializing HttpClient");
            using (var client = new HttpClient(authenticationHandler))
            {

                client.BaseAddress = new Uri(ClientConfig.ApiUrl.AbsoluteUri);
                var boundary = Guid.NewGuid().ToString();

                Logging.Log(TraceEventType.Information, " - Initializing MultipartFormDataContent");
                using (var content = new MultipartFormDataContent(boundary))
                {
                    var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
                    mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
                    content.Headers.ContentType = mediaTypeHeaderValue;

                    {
                        Logging.Log(TraceEventType.Information, "  - Creating XML-body");
                        var xmlMessage = "";
                        xmlMessage = SerializeUtil.Serialize(message);
                        Logging.Log(TraceEventType.Information, String.Format("   -  XML-body \n [{0}]", xmlMessage));
                        var messageContent = new StringContent(xmlMessage);
                        messageContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.digipost-v6+xml");
                        messageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "\"message\""
                        };
                        content.Add(messageContent);
                    }

                    {
                        Logging.Log(TraceEventType.Information, "  - Adding primary document");
                        var documentContent = new ByteArrayContent(message.PrimaryDocument.Content);
                        documentContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                        documentContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = message.PrimaryDocument.Uuid
                        };
                        content.Add(documentContent);
                    }

                    {
                        foreach (Document attachment in message.Attachment)
                        {
                            Logging.Log(TraceEventType.Information, "  - Adding attachment");
                            var attachmentContent = new ByteArrayContent(attachment.Content);
                            attachmentContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                            attachmentContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = attachment.Uuid
                            };
                            content.Add(attachmentContent);
                        }
                    }
                    
                    Logging.Log(TraceEventType.Information, String.Format(" - Posting to URL[{0}]", ClientConfig.ApiUrl+uri));
                    var requestResult = client.PostAsync(uri, content).Result;

                    var contentResult = await requestResult.Content.ReadAsStringAsync();
                    Logging.Log(TraceEventType.Information, String.Format("  - Result \n [{0}]", contentResult));

                    return contentResult;
                }
            }
            
        }

        private static string ComputeHash(Byte[] inputBytes)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

            return Convert.ToBase64String(hashedBytes);
        }



        private static string ComputeSignature(string method, string uri, string date, string sha256Hash, string userId)
        {
            const string parameters = ""; //HttpUtility.UrlEncode(request.RequestUri.Query).ToLower();

            Debug.WriteLine("Canonical string generated by .NET Client:");
            Debug.WriteLine("===START===");

            var s = method.ToUpper() + "\n" +
                    "/" + uri.ToLower() + "\n" +
                    "date: " + date + "\n" +
                    "x-content-sha256: " + sha256Hash + "\n" +
                    "x-digipost-userid: " + userId + "\n" +
                    parameters + "\n";

            Debug.Write(s);
            Debug.WriteLine("===SLUTT===");


            var rsa = GetCert().PrivateKey as RSACryptoServiceProvider;
            var privateKeyBlob = rsa.ExportCspBlob(true);
            var rsa2 = new RSACryptoServiceProvider();
            rsa2.ImportCspBlob(privateKeyBlob);

            var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
            var signature = rsa2.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

            return Convert.ToBase64String(signature);
        }

        private static X509Certificate2 GetCert()
        {
            var storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);
            const string thumbprint = "F7DE9C384EE6D0A81DAD7E8E60BD3776FA5DE9F4";

            return CertificateUtility.SenderCertificate(thumbprint, Language.English);
        }

    }
}
