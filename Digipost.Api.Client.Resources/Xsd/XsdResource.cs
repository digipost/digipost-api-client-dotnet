using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ApiClientShared;

namespace Digipost.Api.Client.Resources.Xsd
{
    internal static class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Api.Client.Resources.Xsd.Data");

        private static XmlReader GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }

        public static XmlReader GetApiV6Xsd()
        {
            return GetResource("api_v6.xsd");
        }
    }
}
