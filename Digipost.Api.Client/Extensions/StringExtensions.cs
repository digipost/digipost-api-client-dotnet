using System.Text.RegularExpressions;

namespace Digipost.Api.Client.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        ///     Removes reserved characters and commonly encoded characters as explained in
        ///     https://en.wikipedia.org/wiki/Percent-encoding
        /// </summary>
        /// <param name="str">string to remove data from</param>
        /// <returns></returns>
        public static string RemoveReservedUriCharacters(this string str)
        {
            var pattern = new Regex("[!#$&'()*+,/:;=?[\\]@%-.<>^_`{|}~]");

            return pattern.Replace(str, "");
        }
    }
}