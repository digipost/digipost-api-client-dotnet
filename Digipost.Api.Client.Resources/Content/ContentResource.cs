using ApiClientShared;

namespace Digipost.Api.Client.Resources.Content
{
    internal class ContentResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Api.Client.Resources.Content.Data");

        private static byte[] GetResource(params string[] path)
        {
            return ResourceUtility.ReadAllBytes(true, path);
        }

        internal static class Hoveddokument
        {
            public static byte[] Pdf()
            {
                return GetResource("Hoveddokument.pdf");
            }

            public static byte[] PlainText()
            {
                return GetResource("Hoveddokument.pdf");
            }
        }

        internal static class Vedlegg
        {
            public static byte[] Pdf()
            {
                return GetResource("Vedlegg.pdf");
            }

            public static byte[] Text()
            {
                return GetResource("Vedlegg.pdf");
            }
        }
    }
}