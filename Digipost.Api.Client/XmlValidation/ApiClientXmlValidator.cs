using Difi.Felles.Utility;
using Digipost.Api.Client.Resources.Xsd;

namespace Digipost.Api.Client.XmlValidation
{
    internal class ApiClientXmlValidator : XmlValidator
    {
        public ApiClientXmlValidator()
        {
            AddXsd(Navnerom.DigipostApiV7, XsdResource.GetApiV7Xsd());
        }
    }
}