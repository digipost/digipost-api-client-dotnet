using Digipost.Api.Client.Resources.Xsd;
using Digipost.Api.Client.Shared.Resources.Resource;
using System.Reflection;

namespace Digipost.Api.Client.Resources.Content
{
    internal class ContentResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(typeof(XsdResource).GetTypeInfo().Assembly,"Digipost.Api.Client.Resources.Content.Data");

        private static byte[] GetResource(params string[] path)
        {
            return ResourceUtility.ReadAllBytes(path);
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
