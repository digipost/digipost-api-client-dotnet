# .NET client library for Digipost API

Online documentation: http://digipost.github.io/digipost-api-client-dotnet/

An example web application that uses this client library can be found at https://github.com/digipost/digipost-client-lib-webapp-dotnet

## Hvordan utvikle på dette prosjektet?
Bruk Rider, som er C#-varianten av Intellij IDEA.

## Auto-Generering av Kode fra XSD
Kjør GenerateClassesFromXsd.ps1 skriptet i Rider, pass på å ha Powershell installert på din maskin.
-  brew cask install powershell

## Hvordan Release?

Releasing er gjort via tagging med [Semver](http://semver.org) versjons schema. For en beta-release, bruk `-beta` som versjon suffix i taggen.
