# .NET client library for Digipost API

Online documentation: http://digipost.github.io/digipost-api-client-dotnet/

## Hvordan utvikle på dette prosjektet?

Bruk Rider, som er C#-varianten av Intellij IDEA.

## Auto-Generering av Kode fra XSD

It should be installed a tool named `xscgen`. Use this to generate the xsd if you need to update the api domain:

`dotnet xscgen "Digipost.Api.Client.Resources/Xsd/Data/api_v8.xsd" -o Digipost.Api.Client.Common/Generated/Apidomain/`

## Hvordan Release?

Releasing er gjort via tagging med [Semver](http://semver.org) versjons schema. For en beta-release, bruk `-beta` som versjon suffix i taggen.
