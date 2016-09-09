using Difi.Felles.Utility;

namespace Digipost.Api.Client.XmlValidation
{
    internal class ApiClientXmlValidator : XmlValidator
    {
        public ApiClientXmlValidator()
        {
            AddXsd(Navnerom.DigipostApiV7, Resources.Xsd.XsdResource.GetApiV7Xsd());
        }
    }
}