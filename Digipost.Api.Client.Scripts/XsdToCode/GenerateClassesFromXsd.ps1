# Running for the first time:
# Install Powershell with command ...
# brew cask install powershell


function GenerateCode($XsdDirectory, $RootXsd, $OutDir, $Namespace)
{
    Write-Host("[Generating code from XSD: $XsdDirectory]")
    Write-Host("Output-directory: $OutDir")
    Write-Host("Changing directory to $XsdDirectory")
    
    Set-Location $XsdDirectory

    Invoke-Expression "xsd $RootXsd /classes /outputdir:$OutDir /namespace:"
}

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$CurrentScriptDir = Get-ScriptDirectory
$SourceDir = (Get-Item $CurrentScriptDir).Parent.Parent.FullName
$XsdToCodeDir = (Get-Item $CurrentScriptDir)
$CurrentApiVersion = "v7"

$CodeDir = "$XsdToCodeDir/Code"
$XsdPath = "$SourceDir/Digipost.Api.Client.Resources/Xsd/Data"
$RootDatatypesXsd = "$XsdPath/datatypes.xsd"
$RootApiXsd = "$XsdPath/api_$CurrentApiVersion.xsd"

GenerateCode "$XsdPath" "$RootDatatypesXsd" "$CodeDir" "datatypes"
GenerateCode "$XsdPath" "$RootApiXsd" "$CodeDir" "$CurrentApiVersion"