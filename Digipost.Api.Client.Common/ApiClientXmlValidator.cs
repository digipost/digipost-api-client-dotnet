using System;
using System.Reflection;
using Digipost.Api.Client.Resources.Xsd;
using Digipost.Api.Client.Shared.Xml;

namespace Digipost.Api.Client.Common
{
    public class ApiClientXmlValidator : XmlValidator
    {
        public ApiClientXmlValidator()
        {
            AddXsd(Namespace.DigipostApiV7, XsdResource.GetApiV7Xsd());

            var dataTypesAssembly = GetDataTypesAssembly();
            if (GetDataTypesAssembly() != null)
            {
                //AddXsd(Namespace.DataTypes, XsdResource.GetDataTypesXsd(dataTypesAssembly));
            }
        }

        private Assembly GetDataTypesAssembly()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Digipost.Api.Client.DataTypes.Core"))
                    return assembly;
            }
            return null;
        }

        public bool CheckIfDataTypesAssemblyIsIncluded()
        {
            return GetDataTypesAssembly() != null;
        }
    }
}
