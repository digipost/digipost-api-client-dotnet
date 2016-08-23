using System.IO;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility;

namespace Digipost.Api.Client.XmlValidation
{
    internal class ApiClientXmlValidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility =
            new ResourceUtility("Digipost.Api.Client.Resources.Xsd");

        public ApiClientXmlValidator()
        {
            AddXsd(Navnerom.DigipostApiInformasjon, XsdResource("api_v6.xsd"));
        }

        private XmlReader XsdResource(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}