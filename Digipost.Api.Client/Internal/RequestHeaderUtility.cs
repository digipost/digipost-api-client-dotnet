using System;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace Digipost.Api.Client.Internal
{
    internal static class RequestHeaderUtility
    {
        public static string ApplyCommonHeaders(HttpRequestMessage request, long brokerId)
        {
            var date = DateTime.UtcNow.ToString("R");

            request.Headers.Add("X-Digipost-UserId", brokerId.ToString());
            request.Headers.Add("Date", date);
            request.Headers.Add("Accept", DigipostVersion.V8);
            request.Headers.Add("User-Agent", GetAssemblyVersion());

            return date;
        }

        public static string RefreshDateHeader(HttpRequestMessage request)
        {
            var date = DateTime.UtcNow.ToString("R");

            request.Headers.Remove("Date");
            request.Headers.Add("Date", date);

            return date;
        }

        public static async Task<string> ApplyContentHashHeaderIfPresent(HttpRequestMessage request)
        {
            if (request.Content == null)
            {
                return null;
            }

            var contentBytes = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var contentHash = ComputeContentHash(contentBytes);
            request.Headers.Add("X-Content-SHA256", contentHash);

            return contentHash;
        }

        public static string ComputeContentHash(byte[] inputBytes)
        {
            IDigest digest = new Sha256Digest();
            var hash = new byte[digest.GetDigestSize()];

            digest.BlockUpdate(inputBytes, 0, inputBytes.Length);
            digest.DoFinal(hash, 0);

            return Convert.ToBase64String(hash);
        }

        public static string GetAssemblyVersion()
        {
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            return $"digipost-api-client-dotnet/{assemblyVersion} ({RuntimeInformation.FrameworkDescription})";
        }
    }
}
