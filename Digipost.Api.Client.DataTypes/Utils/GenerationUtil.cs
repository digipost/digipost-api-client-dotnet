using System;
using System.Collections.Generic;
using XmlSchemaClassGenerator;

namespace Digipost.Api.Client.DataTypes.Utils
{
    public class GenerationUtil
    {
        static void Main(string[] args)
        {
            Generator generator = new Generator
            {
                OutputFolder = "/Users/aaronzachariaharrick/digipost/digipost-api-client-dotnet/Digipost.Api.Client.DataTypes",
                Log = s => Console.Out.WriteLine(s),
                GenerateNullables = true,
                DisableComments = true,
                GenerateInterfaces = true,
                GenerateComplexTypesForCollections = true,
                NamespaceProvider = new Dictionary<NamespaceKey, string>
                {
                    { new NamespaceKey("http://api.digipost.no/schema/datatypes"), "DataTypes" }
                }
                    .ToNamespaceProvider(new GeneratorConfiguration{ NamespacePrefix = "DataTypes" }.NamespaceProvider.GenerateNamespace)
            };
            
            generator.Generate(new List<string>{ "/Users/aaronzachariaharrick/digipost/digipost-api-client-dotnet/Digipost.Api.Client.DataTypes/Resources/XSD/datatypes.xsd" });
        }
    }
}
