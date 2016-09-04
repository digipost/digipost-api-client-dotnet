using Difi.Felles.Utility;

namespace Digipost.Api.Client.XmlValidation
{
    internal class ApiClientXmlValidator : XmlValidator
    {
        public ApiClientXmlValidator()
        {
            AddXsd(Navnerom.DigipostApiInformasjon, Resources.Xsd.XsdResource.GetApiV6Xsd());
        }
    }
}