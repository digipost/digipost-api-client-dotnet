using Difi.Felles.Utility;
using Digipost.Api.Client.Resources.Xsd;

namespace Digipost.Api.Client.Common
{
    internal class ApiClientXmlValidator : XmlValidator
    {
        public ApiClientXmlValidator()
        {
            AddXsd(Namespace.DigipostApiV7, XsdResource.GetApiV7Xsd());
        }
    }
}