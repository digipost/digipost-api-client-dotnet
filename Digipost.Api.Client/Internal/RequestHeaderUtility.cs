using System;
using System.Net.Http;
using System.Reflection;
using System.Runtime;
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

            return $"digipost-api-client-dotnet/{assemblyVersion} (netcore/{GetNetCoreVersion()})";
        }

        private static string GetNetCoreVersion()
        {
            try
            {
                var assembly = typeof(GCSettings).GetTypeInfo().Assembly;
                var assemblyPath = assembly.CodeBase.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
                var netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");

                if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                {
                    return assemblyPath[netCoreAppIndex + 1];
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return "AssemblyVersionNotFound";
        }
    }
}
