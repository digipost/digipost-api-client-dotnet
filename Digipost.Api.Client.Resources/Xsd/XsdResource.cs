using System.IO;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Api.Client.Resources.Xsd
{
    internal static class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(typeof(XsdResource).Assembly, "Digipost.Api.Client.Resources.Xsd.Data");

        private static XmlReader GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return XmlReader.Create(new MemoryStream(bytes));
        }

        public static XmlReader GetApiV7Xsd()
        {
            return GetResource("api_v7.xsd");
        }

        public static XmlReader GetDataTypesXsd()
        {
            return GetResource("datatypes.xsd");
        }
    }
}
